using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("HRCommentCmtee")]
    public class HRCommentCmtee
    {
        public HRCommentCmtee()
        {
            UpdateTime = DateTime.Now;
        }
        [Key]
        public int? ID { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Comment { get; set; }

    }
}
