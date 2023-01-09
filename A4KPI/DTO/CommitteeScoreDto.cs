using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class CommitteeScoreDto
    {
        public CommitteeScoreDto()
        {
            CreatedTime = DateTime.Now;
        }
        public int ID { get; set; }
        public int? CampaignID { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string Score { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
    }
}
