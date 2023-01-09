using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class Language
    {
        [Key]
        [MaxLength(10)]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public virtual ICollection<ModuleTranslation> ModuleTranslations { get; set; }
        public virtual ICollection<FunctionTranslation> FunctionTranslations { get; set; }
    }
}
