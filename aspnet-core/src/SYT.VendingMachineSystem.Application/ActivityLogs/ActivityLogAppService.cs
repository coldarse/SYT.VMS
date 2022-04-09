using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using SYT.VendingMachineSystem.ActivityLogs.Dto;
using Abp.Extensions;

namespace SYT.VendingMachineSystem.ActivityLogs
{
    public class ActivityLogAppService : CrudAppService<ActivityLog, ActivityLogDto, int, PagedActivityLogResultRequestDto>
    {
        public ActivityLogAppService(IRepository<ActivityLog, int> repository) : base(repository)
        {
        }

        protected override IQueryable<ActivityLog> CreateFilteredQuery(PagedActivityLogResultRequestDto input)
        {
            DateTime fromDate = Convert.ToDateTime(input.FromDate);
            DateTime toDate = Convert.ToDateTime(input.ToDate);
            var filteredQuery = Repository.GetAllIncluding()
                .WhereIf(!input.VendingMachine.IsNullOrWhiteSpace(), x => x.VendingMachineName.Contains(input.VendingMachine))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId))
                .Where(x => x.lastUpdatedTime >= fromDate && x.lastUpdatedTime <= toDate.AddDays(1).AddSeconds(-1)); ;

            if (filteredQuery.Any()) return filteredQuery;

            return Repository.GetAllIncluding()
            .WhereIf(!input.VendingMachine.IsNullOrWhiteSpace(), x => x.VendingMachineName.Contains(input.VendingMachine))
            .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId));
        }
    }
}
