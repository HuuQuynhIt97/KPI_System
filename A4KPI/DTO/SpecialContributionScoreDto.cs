using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class SpecialContributionScoreDto
    {
        public int ID { get; set; }
        public string Point { get; set; }
        public int CampaignID { get; set; }
        public int ScoreBy { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public string SpecialScore { get; set; }
        public string Subject { get; set; }
        public string ScoreType { get; set; }
        public string Content { get; set; }
        public List<int> TypeListID { get; set; }
        public List<int> CompactListID { get; set; }

        public int TypeID { get; set; }
        public int CompactID { get; set; }
        public string Ratio { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public bool IsSubmit { get; set; }
    }
}
