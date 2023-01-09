using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace A4KPI.Models
{
    [Table("Actions")]
    public class Action : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Target { get; set; }
        public string Content { get; set; }
        public DateTime? Deadline { get; set; }
        public int AccountId { get; set; }
        public int KPIId { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        public virtual ICollection<ActionStatus> ActionStatus { get; set; }
        public virtual ICollection<Do> Does { get; set; }
    }
}
