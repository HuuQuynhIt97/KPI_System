
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AttitudeScoreDto
    {
        public int ID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int CampaignID { get; set; }
        public string? Comment { get; set; }
        public string AttitudeHeadingName { get; set; }
        public string Score { get; set; }
        public int? ScoreBy { get; set; }
        public DateTime? ScoreTime { get; set; }
        public string HeadingName { get; set; }
    }
    public class ReviseStationDto
    {
        public int UserID { get; set; }
        public int CampaignID { get; set; }
        public string KPIStation { get; set; }
        public string AttStation { get; set; }
    }
    public class SaveScoreDto
    {
        public int CampaignID { get; set; }
        public int HeadingID { get; set; }
        public int ScoreBy { get; set; }
        public int ScoreTo { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public string Score { get; set; }
        public List<ScoreDataDto> Data { get; set; }
        public bool Checked { get; set; }
        public bool L0 { get; set; }
        public bool L1 { get; set; }
        public bool L2 { get; set; }
        public bool FL { get; set; }
        public string TypeBtn { get; set; }
    }

    public class ScoreDataDto
    {
        public int AttCategory { get; set; }
        public int AttHeadingID { get; set; }
        public int BehaviorID { get; set; }
        public int KeypointID { get; set; }
        public int ScoreBy { get; set; }
        public bool Checked { get; set; }

    }

    public class AttitudeScoreDataDto
    {
        public string HeadingName { get; set; }
        public string HeadingCode { get; set; }
        public int HeadingID { get; set; }
        public string Definition { get; set; }
        public object Data { get; set; }
        public int RowSpan { get; set; }
        public string Score { get; set; }
        public string L0Comment { get; set; }
        public string L1Comment { get; set; }
        public string L2Comment { get; set; }
        public string FLComment { get; set; }

        public string L0Score { get; set; }
        public string L1Score { get; set; }
        public string L2Score { get; set; }
        public string FLScore { get; set; }
        public string Comment { get; set; }
        public object File { get; set; }
    }

    public class DataDto
    {
        //public string HeadingName { get; set; }
        public object Passion { get; set; }
        public object Accountbility { get; set; }
        public object Attention { get; set; }
        public object Effective { get; set; }
        public object Resilience { get; set; }
        public object Continuous { get; set; }
        //public object Result { get; set; }
        public string AttitudeHeadingName { get; set; }
        public int CampaignID { get; set; }
        public string Comment { get; set; }
        public string Score { get; set; }
        public int? ScoreBy { get; set; }
        public DateTime? ScoreTime { get; set; }
    }
}
