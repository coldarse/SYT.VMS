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

        private bool _status;
        public bool Status 
        {
            get
            {
                double difference = (DateTime.UtcNow - lastUpdatedTime).TotalMinutes;
                if (difference <= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _status = value;
            }
        }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public DateTime lastUpdatedTime { get; set; }
        public bool Restart { get; set; }

    }
}
