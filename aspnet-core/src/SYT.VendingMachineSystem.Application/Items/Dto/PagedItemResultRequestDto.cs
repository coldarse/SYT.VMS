using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.Items.Dto
{
    public class PagedItemResultRequestDto
    {
        public string Keyword { get; set; }
        public int tenantId { get; set; }
    }
}
