using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SYT.VendingMachineSystem.Configuration;

namespace SYT.VendingMachineSystem.Web.Host.Startup
{
    [DependsOn(
       typeof(VendingMachineSystemWebCoreModule))]
    public class VendingMachineSystemWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VendingMachineSystemWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VendingMachineSystemWebHostModule).GetAssembly());
        }
    }
}
