using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYT.VendingMachineSystem.Sales.Dto;

namespace SYT.VendingMachineSystem.Sales
{
    public class SaleAppService : CrudAppService<Sale, SaleDto, int, PagedSaleResultRequestDto>
    {
        public SaleAppService(IRepository<Sale, int> repository) : base(repository)
        {
        }

        protected override IQueryable<Sale> CreateFilteredQuery(PagedSaleResultRequestDto input)
        {
            DateTime fromDate = Convert.ToDateTime(input.FromDate);
            DateTime toDate = Convert.ToDateTime(input.ToDate);
            var filteredQuery = Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.VendingMachine.Contains(input.Keyword))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId))
                .Where(x => x.OrderTime >= fromDate && x.OrderTime <= toDate.AddDays(1).AddSeconds(-1))
                .OrderByDescending(x => x.Id);

            if(filteredQuery.Any()) return filteredQuery;

            return Repository.GetAllIncluding()
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.VendingMachine.Contains(input.Keyword))
            .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId))
            .OrderByDescending(x => x.Id);
        }

        public List<Sale> getDataForReport(PagedSaleResultRequestDto input)
        {
            DateTime fromDate = Convert.ToDateTime(input.FromDate);
            DateTime toDate = Convert.ToDateTime(input.ToDate);
            var filteredQuery = Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.VendingMachine.Contains(input.Keyword))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId))
                .Where(x => x.OrderTime >= fromDate && x.OrderTime <= toDate.AddDays(1).AddSeconds(-1));

            return filteredQuery.ToList();
        }
    }
}
