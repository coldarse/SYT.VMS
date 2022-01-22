using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.VendingMachines
{
    public class VendingMachine : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public bool isSubscribed { get; set; }
        public bool Status { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public DateTime lastUpdatedTime { get; set; }
    }
}
