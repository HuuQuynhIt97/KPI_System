using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class ReportTrackingAppaisalDto
    {
        public string PIC { get; set; }
        public int Index { get; set; }
        public string L0 { get; set; }
        public string Detail { get; set; }
        public string Campaign { get; set; }
    }
}
