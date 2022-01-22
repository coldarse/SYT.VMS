using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYT.VendingMachineSystem.VendingMachines.Dto;

namespace SYT.VendingMachineSystem.VendingMachines
{
    public class VendingMachineAppService : CrudAppService<VendingMachine, VendingMachineDto, int, PagedVendingMachineResultRequestDto>
    {
        public VendingMachineAppService(IRepository<VendingMachine, int> repository) : base(repository)
        {
        }

        protected override IQueryable<VendingMachine> CreateFilteredQuery(PagedVendingMachineResultRequestDto input)
        {
            return Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword))
                .WhereIf(input.isActive.HasValue, x => x.Status.Equals(input.isActive))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId));

        }
    }
}
