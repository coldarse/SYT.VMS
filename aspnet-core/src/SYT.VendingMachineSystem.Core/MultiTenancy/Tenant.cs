using Abp.MultiTenancy;
using SYT.VendingMachineSystem.Authorization.Users;

namespace SYT.VendingMachineSystem.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
