using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("KPIScoreAttchment")]

    
    public class KPIScoreAttchment
    {
        
        public int ID { get; set; }
        public string Path { get; set; }
        public int CampaignID { get; set; }
        public int CreatedBy { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public int SpecialScoreID { get; set; }
        public string ScoreType { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? UploadTime { get; set; }

    }

}
