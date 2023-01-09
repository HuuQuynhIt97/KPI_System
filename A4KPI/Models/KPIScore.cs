using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("KPIScore")]
    public class KPIScore
    {
        public int ID { get; set; }
        public string Point { get; set; }
        public int CampaignID { get; set; }
        public int ScoreBy { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public bool IsSubmit { get; set; }
        public string ScoreType { get; set; }
        public string Comment { get; set; }
        public DateTime? ScoreTime { get; set; }
        public DateTime? CreatedTime { get; set; }

    }
}
