
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class NewAttitudeDetailDto
    {
        public int ID { get; set; }
        public int? OrderNumber { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Behavior { get; set; }
        public List<NewAttitudeScoreDetailDto> NewAttitudeScoreDetai { get; set; }
        public string Comment { get; set; }
    }

    public class NewAttitudeScoreDetailDto
    {
        public int ID { get; set; }
        public int PointFL { get; set; }
        public int PointL0 { get; set; }
        public int PointL1 { get; set; }
        public int PointL2 { get; set; }
        public int? OrderNumber { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Behavior { get; set; }
    }

    public class NewAttitudeEvaluationDetailDto
    {
        public int ID { get; set; }
        public int CampaignID { get; set; }
        //FL
        public string CommentFL { get; set; }
        public int FLID { get; set; }
        //L2
        public string CommentL2 { get; set; }
        public int L2ID { get; set; }
        //L0
        public bool FirstQuestion1L0 { get; set; }
        public bool FirstQuestion2L0 { get; set; }
        public bool FirstQuestion3L0 { get; set; }
        public bool FirstQuestion4L0 { get; set; }
        public bool FirstQuestion5L0 { get; set; }
        public bool FirstQuestion6L0 { get; set; }
        public bool SecondQuestion1L0 { get; set; }
        public bool SecondQuestion2L0 { get; set; }
        public bool SecondQuestion3L0 { get; set; }
        public bool SecondQuestion4L0 { get; set; }
        public bool SecondQuestion5L0 { get; set; }
        public bool SecondQuestion6L0 { get; set; }
        public string ThirdQuestionL0 { get; set; }
        public string FourthQuestionL0 { get; set; }
        public int L0ID { get; set; }
        //L1
        public bool FirstQuestion1L1 { get; set; }
        public bool FirstQuestion2L1 { get; set; }
        public bool FirstQuestion3L1 { get; set; }
        public bool FirstQuestion4L1 { get; set; }
        public bool FirstQuestion5L1 { get; set; }
        public bool FirstQuestion6L1 { get; set; }
        public bool SecondQuestion1L1 { get; set; }
        public bool SecondQuestion2L1 { get; set; }
        public bool SecondQuestion3L1 { get; set; }
        public bool SecondQuestion4L1 { get; set; }
        public bool SecondQuestion5L1 { get; set; }
        public bool SecondQuestion6L1 { get; set; }
        public string ThirdQuestionL1 { get; set; }
        public int L1ID { get; set; }
    }
}
