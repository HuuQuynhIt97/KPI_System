using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class FunctionSystem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }
        public int Sequence { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }
        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual FunctionSystem Function { get; set; }
        public int? ModuleID { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<FunctionTranslation> FunctionTranslations { get; set; }
    }
}
