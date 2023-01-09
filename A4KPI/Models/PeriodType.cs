using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("PeriodType")]
    public class PeriodType
    {
        public PeriodType(string code, string name, int position)
        {
            Code = code;
            Name = name;
            Position = position;
        }
        public PeriodType(int id, string code, string name, int position)
        {
            Code = code;
            Name = name;
            Position = position;
            Id = id;

        }
     
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int Position { get; set; }
        public int DisplayBefore { get; set; }
    }
}
