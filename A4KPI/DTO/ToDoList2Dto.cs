using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ToDoList2Dto
    {

    }
    public class UpdatePDCADto
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int Month_PDCA { get; set; }
        public int ActionId { get; set; }
        public int DoId { get; set; }
        public string CreatedBy { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Deadline { get; set; }
        public int? StatusId { get; set; }
        public int StatusIdFake { get; set; }
        public int? ActionStatusId { get; set; }
        public string StatusName { get; set; }
        public string DoContent { get; set; }
        public string ResultContent { get; set; }
        public string CContent { get; set; }
        public string AContent { get; set; }
        public string Achievement { get; set; }
        public string ATarget { get; set; }
        public string ADeadline { get; set; }
        public string HeightA { get; set; }
        public string HeightT { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ActCreatedTime { get; set; }

        public int KpiId { get; set; }
        public bool IsDelete { get; set; }


    }
    public class ActionStatusRequestDto
    {
        public int ActionId { get; set; }
        public int StatusId { get; set; }
        public int ActionStatusId { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
