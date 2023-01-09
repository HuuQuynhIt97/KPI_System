using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Evaluation")]
    public class Evaluation
    {
        public Evaluation()
        {
            CreatedTime = DateTime.Now;
            L0SelfScore = "0";
            L1SelfScore = "0";
            L2SelfScore = "0";
            FLSelfScore = "0";
            GmSelfScore = "0";
        }
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CampaignID { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string L0SelfScore { get; set; }
        public string L1SelfScore { get; set; }
        public string L2SelfScore { get; set; }
        public string FLSelfScore { get; set; }
        public string GmSelfScore { get; set; }

    }
}
