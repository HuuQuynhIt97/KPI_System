using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class PeopleCommitteeDto
    {
        public int AppraiseeID { get; set; }
        public string L1Manager { get; set; }
        public int L1ID { get; set; }
        public string Appraisee { get; set; }
        public string Center { get; set; }
        public string Dept { get; set; }
        public int Index { get; set; }
    }
}
