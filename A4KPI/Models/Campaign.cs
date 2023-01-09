using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Campaign")]
    public class Campaign
    {
        public Campaign()
        {
            CreatedTime = DateTime.Now;
        }
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string MonthName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string Year { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }

        public bool IsStart { get; set; }


    }
}
