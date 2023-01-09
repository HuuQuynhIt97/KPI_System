using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("SpecialRatio")]
    public class SpecialRatio
    {
        public int ID { get; set; }
        public string Point { get; set; }

    }
}
