using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class PeopleCommitteeSpecialScoreDto
    {
        public int ID { get; set; }
        public int ScoreTo { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public int TypeID { get; set; }
        public int CompactID { get; set; }
        public string Ratio { get; set; }
        public string Point { get; set; }
        public object Files { get; set; }
    }
}
