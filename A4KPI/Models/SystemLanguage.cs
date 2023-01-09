using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("SystemLanguages")]
    public class SystemLanguage
    {
        [Key]
        public int ID { get; set; }
        public string SLPage { get; set; }
        public string SLType { get; set; }
        public string SLKey { get; set; }
        public string Comment { get; set; }
        public string SLTW { get; set; }
        public string SLEN { get; set; }

    }
}
