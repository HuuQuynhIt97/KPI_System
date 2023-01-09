using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class PeopleCommitteeKPIDetailDto
    {
        public List<object> KPIDefaultPerson { get; set; }
        public List<object> KPIStringPerson { get; set; }
        public List<object> KPIDefaultMuti { get; set; }
        public List<object> KPIStringMuti { get; set; }
    }
}
