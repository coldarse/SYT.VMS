using System.Threading.Tasks;
using Abp.Application.Services;
using SYT.VendingMachineSystem.Sessions.Dto;

namespace SYT.VendingMachineSystem.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
