using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("NewAttitudeAttchment")]
    public class NewAttitudeAttchment
    {
        [Key]
        public int ID { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? OrderNumber { get; set; }
        public string Comment { get; set; }
    }
}
