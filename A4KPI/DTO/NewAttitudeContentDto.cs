
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class NewAttitudeContentDto
    {
        public int ID { get; set; }
        public int? OrderNumber { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Behavior { get; set; }
        public int? NewAttitudeAttchmentID { get; set; }
        public string Comment { get; set; }
        public List<NewAttitudeScoreDto> NewAttitudeScore{ get; set; }
    }
}
