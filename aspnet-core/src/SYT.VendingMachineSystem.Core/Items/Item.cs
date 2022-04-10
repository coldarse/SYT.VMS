using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.Items
{
    public class Item : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string VendingMachine { get; set; }
        public string ItemCode { get; set; }
    }
}
