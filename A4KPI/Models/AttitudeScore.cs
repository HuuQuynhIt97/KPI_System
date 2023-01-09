using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("AttitudeScore")]
    public class AttitudeScore
    {
        public int ID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int CampaignID { get; set; }
        public string? Comment { get; set; }

        public string Score { get; set; }
        public int? ScoreBy { get; set; }
        public DateTime? ScoreTime { get; set; }

    }
}
