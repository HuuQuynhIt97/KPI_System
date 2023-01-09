using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class KPIScoreDto
    {
        public int ID { get; set; }
        public string Point { get; set; }
        public int CampaignID { get; set; }
        public int ScoreBy { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public string ScoreType { get; set; }
        public string Comment { get; set; }
        public bool ButtonIsSubmit { get; set; }
        public bool IsSubmit { get; set; }
        public DateTime? ScoreTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string HRComment { get; set; }
        public int HRCommentCreatedBy { get; set; }
        public int HRCommentUpdateBy { get; set; }
    }
}
