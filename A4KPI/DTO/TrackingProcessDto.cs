using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class TrackingProcessDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Center { get; set; }
        public string Dept { get; set; }
        public int TodoTotal { get; set; }
        public int TodoPending { get; set; }
        public string Percentage { get; set; }

    }

    public class TrackingProcessDataKPIDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string  KPIName { get; set; }
        public int Index { get; set; }

    }

    public class TrackingProcessDataDto
    {
        public List<TrackingProcessDto> TotalTracking { get; set; }
        public double TodoTotal { get; set; }
        public double TodoPending { get; set; }
        public double Percentage { get; set; }

    }
}
