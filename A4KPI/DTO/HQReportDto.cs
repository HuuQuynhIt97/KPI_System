using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class HQReportDto
    {
        public int ID { get; set; }
        public string Factory { get; set; }
        public string Division { get; set; }
        public string Dept { get; set; }
        public string L2Name { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public double? AttitudeScore { get; set; }
        public double? KpiScore { get; set; }
        public double? SpecialScore { get; set; }
        public double? H1Score { get; set; }
        public int Index {get; set;}
    }
}
