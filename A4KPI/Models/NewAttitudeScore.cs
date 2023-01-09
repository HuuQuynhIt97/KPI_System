using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("NewAttitudeScore")]
    public class NewAttitudeScore
    {
        [Key]
        public int ID { get; set; }
        public bool Point1 { get; set; }
        public bool Point2 { get; set; }
        public bool Point3 { get; set; }
        public bool Point4 { get; set; }
        public bool Point5 { get; set; }
        public bool Point6 { get; set; }
        public bool Point7 { get; set; }
        public bool Point8 { get; set; }
        public bool Point9 { get; set; }
        public bool Point10 { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? AttitudeContenID { get; set; }
        public int? OrderNumber { get; set; }
    }
}
