using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class DoDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string ReusltContent { get; set; }
        public string Achievement { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int ActionId { get; set; }
    }
}
