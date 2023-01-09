using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("NewAttitudeContent")]
    public class NewAttitudeContent
    {
        [Key]
        public int ID { get; set; }
        public int? OrderNumber { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Behavior { get; set; }
    }
}
