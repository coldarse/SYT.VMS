using System.Threading.Tasks;
using SYT.VendingMachineSystem.Configuration.Dto;

namespace SYT.VendingMachineSystem.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
