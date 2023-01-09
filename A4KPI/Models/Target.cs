using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Targets")]
    public class Target: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; } // this month target
        public double Performance { get; set; } // this month performance
        public double YTD { get; set; }
        public int KPIId { get; set; }
         public bool Submitted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime TargetTime { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }

        public virtual ICollection<TargetPIC> TargetPICs { get; set; }


    }
}
