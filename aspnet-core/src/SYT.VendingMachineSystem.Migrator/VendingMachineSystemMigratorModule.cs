using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SYT.VendingMachineSystem.Configuration;
using SYT.VendingMachineSystem.EntityFrameworkCore;
using SYT.VendingMachineSystem.Migrator.DependencyInjection;

namespace SYT.VendingMachineSystem.Migrator
{
    [DependsOn(typeof(VendingMachineSystemEntityFrameworkModule))]
    public class VendingMachineSystemMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public VendingMachineSystemMigratorModule(VendingMachineSystemEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(VendingMachineSystemMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                VendingMachineSystemConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VendingMachineSystemMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
