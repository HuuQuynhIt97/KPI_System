using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AccountCampaignDto
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }
        public int FL { get; set; }
        public bool IsL0 { get; set; }
        public int CampaignID { get; set; }
        public int SystemFlow { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
