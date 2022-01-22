using Abp.Authorization;
using SYT.VendingMachineSystem.Authorization.Roles;
using SYT.VendingMachineSystem.Authorization.Users;

namespace SYT.VendingMachineSystem.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
