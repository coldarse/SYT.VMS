using Abp.Application.Services.Dto;

namespace SYT.VendingMachineSystem.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

