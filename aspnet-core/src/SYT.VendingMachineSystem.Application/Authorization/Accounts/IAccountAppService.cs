using System.Threading.Tasks;
using Abp.Application.Services;
using SYT.VendingMachineSystem.Authorization.Accounts.Dto;

namespace SYT.VendingMachineSystem.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
