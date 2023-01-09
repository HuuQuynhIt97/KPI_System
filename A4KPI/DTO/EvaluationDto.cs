using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class EvaluationDto
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string L0SelfScore { get; set; }
        public int CampaignID { get; set; }
        public int L0ID { get; set; }
        public int FLID { get; set; }
        public int L1ID { get; set; }
        public int L2ID { get; set; }
        public string CampaignName { get; set; }
        public string L1SelfScore { get; set; }
        public string L2SelfScore { get; set; }
        public string Type { get; set; }
        public string FLSelfScore { get; set; }
        public string GmSelfScore { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }
        public string Dept { get; set; }
        public string Center { get; set; }
        public object test { get; set; }



        //Attitude-button
        public bool isDisplayAttitudeBtnFL { get; set; }
        public bool isDisplayAttitudeBtnL0 { get; set; }
        public bool isDisplayAttitudeBtnL1 { get; set; }
        public bool isDisplayAttitudeBtn { get; set; }

        //KPI-button
        public bool isDisplayKPIBtnFL { get; set; }
        public bool isDisplayKPIBtnL0 { get; set; }
        public bool isDisplayKPIBtnL1 { get; set; }
        public bool isDisplayKPIBtn { get; set; }

        public bool isGM { get; set; }
        public bool BtnAttitude { get; set; }
        public bool BtnKPI { get; set; }
        public bool BtnNewAttitude { get; set; }

        //New-Attitude-button
        public bool isDisplayNewAttBtnFL { get; set; }
        public bool isDisplayNewAttBtnL0 { get; set; }
        public bool isDisplayNewAttBtnL1 { get; set; }
        public bool isDisplayNewAttBtn { get; set; }
        
    }

    
}
