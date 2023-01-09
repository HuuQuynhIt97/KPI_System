
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class PerfomanceEvaluationTypeDto
    {
        public int ID { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public int? TypeID { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
