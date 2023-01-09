using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class BehaviorCheckDto
    {
        public int ID { get; set; }
        public int BehaviorID { get; set; }
        public int? CampaignID { get; set; }
        public DateTime? CheckTime { get; set; }
        public int CheckBy { get; set; }
        public int CheckFrom { get; set; }
        public int CheckTo { get; set; }

        public bool Checked { get; set; }
        public bool L0Checked { get; set; }
        public bool L1Checked { get; set; }
        public bool L2Checked { get; set; }
        public bool FLChecked { get; set; }
    }
}
