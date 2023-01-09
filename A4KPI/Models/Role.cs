using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Roles")]
    public class Role
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
