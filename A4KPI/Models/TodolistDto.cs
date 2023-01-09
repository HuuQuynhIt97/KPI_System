using A4KPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class ActionRequestDto
    {
        public List<ActionDto> Actions { get; set; }
        public TargetDto Target { get; set; }
        public TargetYTDDto TargetYTD { get; set; }
        public DateTime CurrentTime { get; set; }
        public int UserId { get; set; }
    }
    public class PDCARequestDto
    {
        //public Result Result { get; set; }
        public TargetDto Target { get; set; }
        public TargetDto NextMonthTarget { get; set; }
        public TargetYTDDto TargetYTD { get; set; }
        public List<ActionDto> Actions { get; set; }
        public List<UpdatePDCADto> UpdatePDCA { get; set; }
        public int UserId { get; set; }
        public DateTime CurrentTime { get; set; }

    }

}
