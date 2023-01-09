using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("SpecialType")]
    public class SpecialType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameZh { get; set; }

    }
}
