using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace SYT.VendingMachineSystem.Controllers
{
    public abstract class VendingMachineSystemControllerBase: AbpController
    {
        protected VendingMachineSystemControllerBase()
        {
            LocalizationSourceName = VendingMachineSystemConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
