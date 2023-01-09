using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class UserSystemFlow
    {
        public int ID { get; set; }
        public int SystemFlowID { get; set; }
        public string Value { get; set; }
        public bool IsBtnKPI { get; set; }
        public bool IsBtnAtt { get; set; }
    }
}
