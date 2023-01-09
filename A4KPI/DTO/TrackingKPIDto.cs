using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class TrackingKPIDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string PICName { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public string Year { get; set; }
        public int Index { get; set; }
        public bool Status { get; set; }
        public bool CurrentTarget { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

    }

   
}
