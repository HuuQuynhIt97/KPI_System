using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("SpecialContributionScore")]
    public class SpecialContributionScore
    {
        public int ID { get; set; }
        public string Point { get; set; }
        public int CampaignID { get; set; }
        public int ScoreBy { get; set; }
        public string ScoreType { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public string SpecialScore { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int TypeID { get; set; }
        public int CompactID { get; set; }
        public string Ratio { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool IsSubmit { get; set; }
    }
}
