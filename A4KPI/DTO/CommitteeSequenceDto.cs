using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class CommitteeSequenceDto
    {
        public int ID { get; set; }
        public int Sequence { get; set; }
        public int CampaignID { get; set; }
        public int AppraiseeID { get; set; }
        public int ToIndex { get; set; }
        public int FromIndex { get; set; }
        public bool IsUpdate { get; set; }
        
    }
}
