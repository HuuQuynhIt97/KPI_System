using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ChartDto
    {
        public string[] labels { get; set; }
        public double[] perfomances { get; set; }
        public double[] targets { get; set; }
        public double YTD { get; set; }
        public object DataTable { get; set; }
    }
    public class ChartDtoDateTime
    {
        public string[] labels { get; set; }
        public double[] perfomances { get; set; }
        public double[] targets { get; set; }
        public double[] ytds { get; set; }
        public double YTD { get; set; }
        public double TargetYTD { get; set; }
        public object DataTable { get; set; }
        public int TypeId { get; set; }
        public string Policy { get; set; }
        public bool Status { get; set; }
    }

    public class DataTable
    {
        public string Month { get; set; }
        public string Date { get; set; }
        public int KpiId { get; set; }
        public string Content { get; set; }
        public object CurrentMonthData { get; set; }
        public object NextMonthData { get; set; }
        public object Targets { get; set; }
        public object Deadline { get; set; }
        public object Archievement { get; set; }
        public object Status { get; set; }
    }

    public class DataTablePDCA
    {
        public string Month { get; set; }
        public string Date { get; set; }
        public int KpiId { get; set; }
        public string Content { get; set; }
        public List<UpdatePDCADto> CurrentMonthData { get; set; }
        public object NextMonthData { get; set; }
        public object Targets { get; set; }
        public object Deadline { get; set; }
        public object Archievement { get; set; }
        public object Status { get; set; }
    }
    public class DataTable2
    {
        public string Month { get; set; }
        public string Date { get; set; }
        public int KpiId { get; set; }
        public string Content { get; set; }
        public List<UpdatePDCADto> CurrentMonthData { get; set; }
        public List<UpdatePDCADto> NextMonthData { get; set; }
        public object Targets { get; set; }
        public object Deadline { get; set; }
        public object Archievement { get; set; }
        public object Status { get; set; }
    }

    public class ResultsDto
    {
        public string Month { get; set; }
        public string Date { get; set; }
        public int KpiId { get; set; }
        public string Content { get; set; }
        public string ContentA { get; set; }
        public string TargetsA { get; set; }

        public object Targets { get; set; }
        public object Deadline { get; set; }
        public string DeadlineA { get; set; }
        public object Archievement { get; set; }
        public object Status { get; set; }
    }
}
