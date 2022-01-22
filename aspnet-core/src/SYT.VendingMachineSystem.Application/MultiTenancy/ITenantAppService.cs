using Abp.Application.Services;
using SYT.VendingMachineSystem.MultiTenancy.Dto;

namespace SYT.VendingMachineSystem.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

