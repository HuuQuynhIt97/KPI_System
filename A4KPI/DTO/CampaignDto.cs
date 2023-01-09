using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class CampaignDto
    {
       
        
        public int ID { get; set; }
        public string  Name { get; set; }
        public string  MonthName { get; set; }
        public int CreatedBy { get; set; }

        public string Creator { get; set; }

        public string CreatedTime { get; set; }
        public string Year { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public bool IsStart { get; set; }

        
        
    }
}
