using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class VersionDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UploadBy { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
