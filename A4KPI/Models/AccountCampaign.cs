using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("AccountCampaign")]
    public class AccountCampaign
    {
        public AccountCampaign()
        {
            CreatedTime = DateTime.Now;
        }
        [Key]
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }
        public int FL { get; set; }
        public bool? IsL0 { get; set; }
        public int CampaignID { get; set; }
        public int? SystemFlow { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
