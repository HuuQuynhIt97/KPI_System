using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class NewAttitudeSubmit
    {
        public int ID { get; set; }
        public int CampaignID { get; set; }
        public int SubmitFrom { get; set; }
        public int SubmitTo { get; set; }

        public bool FLAttitude { get; set; }
        public bool L0Attitude { get; set; }
        public bool L1Attitude { get; set; }
        public bool L2Attitude { get; set; }

        public bool FLKPI { get; set; }
        public bool L0KPI { get; set; }
        public bool L1KPI { get; set; }
        public bool L2KPI { get; set; }

        public bool IsDisplayFL { get; set; }
        public bool IsDisplayL0 { get; set; }
        public bool IsDisplayL1 { get; set; }
        public bool IsDisplayL2 { get; set; }

        public bool BtnFL { get; set; }
        public bool BtnL0 { get; set; }
        public bool BtnL1 { get; set; }
        public bool BtnL2 { get; set; }

        public bool BtnFLKPI { get; set; }
        public bool BtnL0KPI { get; set; }
        public bool BtnL1KPI { get; set; }
        public bool BtnL2KPI { get; set; }

        public bool IsSubmitAttitudeFL { get; set; }
        public bool IsSubmitAttitudeL0 { get; set; }
        public bool IsSubmitAttitudeL1 { get; set; }
        public bool IsSubmitAttitudeL2 { get; set; }

        public DateTime? SubmitTime { get; set; }
    }
}
