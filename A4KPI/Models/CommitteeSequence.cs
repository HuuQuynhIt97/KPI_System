using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("CommitteeSequence")]
    public class CommitteeSequence
    {
        [Key]
        public int ID { get; set; }
        public int Sequence { get; set; }
        public int CampaignID { get; set; }
        public int AppraiseeID { get; set; }
        public bool? IsUpdate { get; set; }

    }
}
