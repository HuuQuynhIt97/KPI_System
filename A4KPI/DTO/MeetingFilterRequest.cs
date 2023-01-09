using System;

namespace A4KPI.DTO
{
    public class MeetingFilterRequest
    {
        public string Factory { get; set; }
        public string Center { get; set; }
        public string Dept { get; set; }
        public int Level { get; set; }
    }
    
}
