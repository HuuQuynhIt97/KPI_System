using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace A4KPI.Models
{
    [Table("Versions")]
    public class Version
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string UploadBy { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
