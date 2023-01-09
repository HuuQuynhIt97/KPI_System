using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace A4KPI.Models
{
    [Table("ActionStatus")]
    public class ActionStatus : IDateTracking
    {
        [Key]
        public int Id { get; set; } 
        public int StatusId { get; set; }
        public int ActionId { get; set; }
        public bool Submitted { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(ActionId))]
        public virtual Action Action { get; set; }
    }
}
