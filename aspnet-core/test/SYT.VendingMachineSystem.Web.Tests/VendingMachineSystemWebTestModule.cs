using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SYT.VendingMachineSystem.EntityFrameworkCore;
using SYT.VendingMachineSystem.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace SYT.VendingMachineSystem.Web.Tests
{
    [DependsOn(
        typeof(VendingMachineSystemWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class VendingMachineSystemWebTestModule : AbpModule
    {
        public VendingMachineSystemWebTestModule(VendingMachineSystemEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VendingMachineSystemWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(VendingMachineSystemWebMvcModule).Assembly);
        }
    }
}