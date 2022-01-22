using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using SYT.VendingMachineSystem.Configuration.Dto;

namespace SYT.VendingMachineSystem.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : VendingMachineSystemAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
