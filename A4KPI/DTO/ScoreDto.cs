using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ScoreDto
    {
        public int ID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int CampaignID { get; set; }
        public string L0Comment { get; set; }
        public string L1Comment { get; set; }
        public string L2Comment { get; set; }
        public string FlComment { get; set; }
        public float? L0Score { get; set; }
        public float? L1Score { get; set; }
        public float? L2Score { get; set; }
        public float? FLScore { get; set; }
        public string Point { get; set; }
        public int ScoreBy { get; set; }
        public DateTime? ScoreTime { get; set; }
        public int ScoreTo { get; set; }
    }
}
