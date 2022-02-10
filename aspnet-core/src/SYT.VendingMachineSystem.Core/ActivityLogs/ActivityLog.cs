using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.ActivityLogs
{
    public class ActivityLog : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int VendingMachineId { get; set; }
        public string VendingMachineName { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime lastUpdatedTime { get; set; }
        public string ItemCode { get; set; }
    }
}
