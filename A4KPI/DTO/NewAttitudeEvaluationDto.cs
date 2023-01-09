
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class NewAttitudeEvaluationDto
    {
        public int ID { get; set; }
        public int? CampaignID { get; set; }
        public int? ScoreTo { get; set; }
        public int? ScoreFrom { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string Type { get; set; }
        public bool FirstQuestion1 { get; set; }
        public bool FirstQuestion2 { get; set; }
        public bool FirstQuestion3 { get; set; }
        public bool FirstQuestion4 { get; set; }
        public bool FirstQuestion5 { get; set; }
        public bool FirstQuestion6 { get; set; }
       
        public bool SecondQuestion1 { get; set; }
        public bool SecondQuestion2 { get; set; }
        public bool SecondQuestion3 { get; set; }
        public bool SecondQuestion4 { get; set; }
        public bool SecondQuestion5 { get; set; }
        public bool SecondQuestion6 { get; set; }
        
        public string ThirdQuestion { get; set; }
        public string FourthQuestion { get; set; }
    }
}
