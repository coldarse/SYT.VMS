using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.ActivityLogs.Dto
{
    public class PagedActivityLogResultRequestDto
    {
        public string VendingMachine { get; set; }
        public int tenantId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
