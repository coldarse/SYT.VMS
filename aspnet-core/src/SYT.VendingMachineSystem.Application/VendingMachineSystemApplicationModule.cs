using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SYT.VendingMachineSystem.Authorization;

namespace SYT.VendingMachineSystem
{
    [DependsOn(
        typeof(VendingMachineSystemCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class VendingMachineSystemApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<VendingMachineSystemAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(VendingMachineSystemApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
