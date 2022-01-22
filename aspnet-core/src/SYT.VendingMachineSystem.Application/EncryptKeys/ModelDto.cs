using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYT.VendingMachineSystem.EncryptKeys
{
    public class ModelDto
    {
    }

    public class vendingMachineDto
    {
        public string name { get; set; }
        public bool status { get; set; }
    }

    public class activityLogDto
    {
        public string vendingMachineName { get; set; }
        public string activityDescription { get; set; }
    }
}
