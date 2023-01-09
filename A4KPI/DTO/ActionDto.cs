using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ActionDto
    {
        public int Id { get; set; }
        public string Target { get; set; }
        public string Content { get; set; }

        public DateTime? Deadline { get; set; }
        public int AccountId { get; set; }
        public int KPIId { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
