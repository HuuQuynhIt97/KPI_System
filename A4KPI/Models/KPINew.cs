using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{


    [Table("KPINew")]
    public class KPINew : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int Level { get; set; }
        public int? OcId { get; set; }
        public int? PolicyId { get; set; }
        public int TypeId { get; set; }
        public int CreateBy { get; set; }
        public int? LevelOcCreateBy { get; set; }
        public int? OcIdCreateBy { get; set; }
        public int Pic { get; set; }
        public int? ParentId { get; set; }
        public bool Submitted { get; set; }
        public bool IsDisplayTodo { get; set; }

        public int UpdateBy { get; set; }
        public int? Sequence { get; set; }
        public int? SequenceCHM { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? StartDisplayMeetingTime { get; set; }
        public DateTime? EndDisplayMeetingTime { get; set; }

        //[ForeignKey(nameof(PolicyId))]
        //public virtual Policy  Policy{ get; set; }
        public virtual ICollection<Action> Actions{ get; set; }
        public virtual ICollection<Target> Targets{ get; set; }

        public virtual ICollection<KPIAccount> KPIAccounts{ get; set; }

    }
}
