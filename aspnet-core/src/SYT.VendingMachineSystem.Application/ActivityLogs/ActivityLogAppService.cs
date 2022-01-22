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
            return Repository.GetAllIncluding()
                .WhereIf(!input.VendingMachine.IsNullOrWhiteSpace(), x => x.VendingMachineName.Contains(input.VendingMachine))
                .WhereIf(input.tenantId != 1, x => x.TenantId.Equals(input.tenantId));
        }
    }
}
