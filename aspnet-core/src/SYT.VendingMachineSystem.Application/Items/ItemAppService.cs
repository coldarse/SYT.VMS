using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using SYT.VendingMachineSystem.Items.Dto;

namespace SYT.VendingMachineSystem.Items
{
    public class ItemAppService : CrudAppService<Item, ItemDto, int, PagedItemResultRequestDto>
    {
        public ItemAppService(IRepository<Item, int> repository) : base(repository)
        {
        }

        protected override IQueryable<Item> CreateFilteredQuery(PagedItemResultRequestDto input)
        {
            return Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.VendingMachine.Contains(input.Keyword))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId));

        }
    }
}
