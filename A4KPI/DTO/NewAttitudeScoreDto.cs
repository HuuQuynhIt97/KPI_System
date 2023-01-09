
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class NewAttitudeScoreDto
    {
        public int ID { get; set; }
        public bool Point1 { get; set; }
        public bool Point2 { get; set; }
        public bool Point3 { get; set; }
        public bool Point4 { get; set; }
        public bool Point5 { get; set; }
        public bool Point6 { get; set; }
        public bool Point7 { get; set; }
        public bool Point8 { get; set; }
        public bool Point9 { get; set; }
        public bool Point10 { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? AttitudeContenID { get; set; }
        public int? OrderNumber { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Behavior { get; set; }
    }

    public class NewAttitudePointDto
    {
        public int ID { get; set; }
        
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }

        public int? OrderNumber { get; set; }
        public int Point { get; set; }


    }
}
