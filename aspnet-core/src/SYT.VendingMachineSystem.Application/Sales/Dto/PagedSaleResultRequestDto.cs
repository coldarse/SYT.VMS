﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.Sales.Dto
{
    public class PagedSaleResultRequestDto
    {
        public string Keyword { get; set; }
        public int tenantId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
