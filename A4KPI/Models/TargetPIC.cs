using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("TargetPIC")]
    public class TargetPIC
    {

        [Key]
        public int Id { get; set; }
        public int targetId { get; set; }
        public int AccountId { get; set; }
        public bool IsSubmit { get; set; }

        [ForeignKey(nameof(targetId))]
        public virtual Target Target { get; set; }


    }
}
