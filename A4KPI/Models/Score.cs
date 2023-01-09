using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class Score
    {
        public int ID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int CampaignID { get; set; }
        public string L0Comment { get; set; }
        public string L1Comment { get; set; }
        public string L2Comment { get; set; }
        public string FlComment { get; set; }

        public string L0Score { get; set; }
        public string L1Score { get; set; }
        public string L2Score { get; set; }
        public string FLScore { get; set; }

        public string Point { get; set; }
        public int ScoreBy { get; set; }
        public DateTime? ScoreTime { get; set; }
        public int ScoreTo { get; set; }
    }
}
