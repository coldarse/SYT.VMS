﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using SYT.VendingMachineSystem.Authentication.External;
using SYT.VendingMachineSystem.Authentication.JwtBearer;
using SYT.VendingMachineSystem.Authorization;
using SYT.VendingMachineSystem.Authorization.Users;
using SYT.VendingMachineSystem.Models.TokenAuth;
using SYT.VendingMachineSystem.MultiTenancy;
using Abp.Domain.Repositories;
using SYT.VendingMachineSystem.VendingMachines;
using SYT.VendingMachineSystem.EncryptKeys;
using SYT.VendingMachineSystem.ActivityLogs;
using SYT.VendingMachineSystem.Sales;
using Abp.Domain.Uow;
using SYT.VendingMachineSystem.Items;

namespace SYT.VendingMachineSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : VendingMachineSystemControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IRepository<VendingMachine> _VendingMachineRepository;
        private readonly IRepository<ActivityLog> _ActivityLogRepository;
        private readonly IRepository<Sale> _SaleRepository;
        private readonly IRepository<Item> _ItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            IRepository<VendingMachine> VendingMachineRepository,
            IRepository<ActivityLog> ActivityLogRepository,
            IRepository<Sale> SaleRepository,
            IRepository<Item> ItemRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _VendingMachineRepository = VendingMachineRepository;
            _ActivityLogRepository = ActivityLogRepository;
            _SaleRepository = SaleRepository;
            _ItemRepository = ItemRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #region CustomAPI

        [HttpGet]
        public async Task<itemByVending> getItemCodes(string vendingMachineName)
        {
            List<VendingMachine> tempVending = _VendingMachineRepository.GetAll().ToList();

            foreach (var tv in tempVending)
            {
                if (tv.Name == vendingMachineName)
                {
                    if (tv.isSubscribed == true)
                    {
                        itemByVending ic = new itemByVending();
                        ic.vendingMachineName = vendingMachineName;

                        List<Item> tempItem = _ItemRepository.GetAll().Where(x => x.VendingMachine == vendingMachineName).ToList();
                        ic.itemCodes = new List<string>();
                        foreach(var item in tempItem)
                        {
                            ic.itemCodes.Add(item.ItemCode);
                        }
                        return ic;
                    }
                    else
                    {
                        throw new UserFriendlyException("Vending Machine is not Subscribed!", "401");
                    }
                }
            }
            throw new UserFriendlyException("Vending Machine is not Found!", "401");
        }

        [HttpPut]
        public async void updateVendingMachineStatus(vendingMachineDto vendingMachine)
        {
            List<VendingMachine> tempVending = _VendingMachineRepository.GetAll().ToList();

            foreach(var tv in tempVending)
            {
                if(tv.Name == vendingMachine.name)
                {
                    if (tv.isSubscribed == true)
                    {
                        VendingMachine vm = new VendingMachine();
                        vm = tv;
                        vm.Status = vendingMachine.status;
                        vm.lastUpdatedTime = DateTime.Now;

                        await _VendingMachineRepository.UpdateAsync(vm);
                        break;
                    }
                    break;
                }
            }
        }    

        [HttpPost]
        public async Task<ActivityLog> addToActivityLog(activityLogDto activityLog)
        {

            List<VendingMachine> tempVending = _VendingMachineRepository.GetAll().ToList();

            foreach(var temp in tempVending)
            {
                if(temp.Name == activityLog.vendingMachineName)
                {
                    if (temp.isSubscribed)
                    {
                        ActivityLog al = new ActivityLog();
                        al.TenantId = temp.TenantId;
                        al.VendingMachineId = temp.Id;
                        al.VendingMachineName = temp.Name;
                        al.ActivityDescription = activityLog.activityDescription;
                        al.lastUpdatedTime = DateTime.Now;

                        return await _ActivityLogRepository.InsertAsync(al);
                    }
                    else
                    {
                        throw new UserFriendlyException("Vending Machine is not Subscribed!", "401");
                    }
                }
            }
            throw new UserFriendlyException("Vending Machine is not Found!", "401");
        }

        [HttpPost]
        public async Task<Sale> addToSales(saleDto sale)
        {
            List<VendingMachine> tempVending = _VendingMachineRepository.GetAllList();

            foreach (var temp in tempVending)
            {
                if (temp.Name == sale.vendingMachineName)
                {
                    if (temp.isSubscribed)
                    {
                        Sale so = new Sale();
                        so.TenantId = temp.TenantId;
                        so.VendingMachine = temp.Name;
                        so.ItemCode = sale.itemCode;
                        so.OrderTime = DateTime.Now;

                        return await _SaleRepository.InsertAsync(so);
                    }
                    else
                    {
                        throw new UserFriendlyException("Vending Machine is not Subscribed!", "401");
                    }
                }
            }
            throw new UserFriendlyException("Vending Machine is not Found!", "401");
        }

        #endregion

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id
            };
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        // Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                Authorization.Users.User.CreateRandomPassword(),
                true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken);
        }
    }
}
