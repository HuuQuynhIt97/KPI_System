
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class KeypointDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AttitudeCategoryID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int Level { get; set; }
    }
}
