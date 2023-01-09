
﻿using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AttitudeAttchmentDto
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public int CampaignID { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UploadTime { get; set; }
    }
}
