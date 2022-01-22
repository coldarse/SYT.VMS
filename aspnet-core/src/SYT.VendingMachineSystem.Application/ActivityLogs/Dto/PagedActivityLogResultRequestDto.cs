using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.ActivityLogs.Dto
{
    public class PagedActivityLogResultRequestDto
    {
        public string Keyword { get; set; }
        public string VendingMachine { get; set; }
        public int tenantId { get; set; }
    }
}
