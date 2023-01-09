using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("PerfomanceEvaluationType")]
    public class PerfomanceEvaluationType
    {
        [Key]
        public int ID { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public int? TypeID { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedTime { get; set; }

    }
}
