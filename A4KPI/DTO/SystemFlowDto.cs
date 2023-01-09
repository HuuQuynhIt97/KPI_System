using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class SystemFlowDto
    {
        public int ID { get; set; }
        public int SystemFlowID { get; set; }
        public bool FL { get; set; }
        public bool L0 { get; set; }
        public bool L1 { get; set; }
        public bool L2 { get; set; }
    }
}
