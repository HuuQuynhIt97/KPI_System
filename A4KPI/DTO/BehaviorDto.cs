
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class BehaviorDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AttitudeKeypointID { get; set; }
        public int AttitudeHeadingID { get; set; }
    }
}
