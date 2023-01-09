using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{

    [Table("PIC")]
    public class PIC
    {
        [Key]
        public int Id { get; set; }
        public int ObjectiveId { get; set; }
        public int AccountId { get; set; }
     
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
       
    }
}
