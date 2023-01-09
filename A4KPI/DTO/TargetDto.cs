using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class TargetDto
    {
        public int Id { get; set; }
        public double Value { get; set; } // this month target
        public double Performance { get; set; } // this month performance
        public double YTD { get; set; }
        public int KPIId { get; set; }
        public int CreatedBy { get; set; }
        public bool Submitted { get; set; }

        public DateTime TargetTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    public class TargetL0Dto
    {
        public double Performance { get; set; } // this month performance

        public bool Submitted { get; set; }

        public DateTime TargetTime { get; set; }

    }
}