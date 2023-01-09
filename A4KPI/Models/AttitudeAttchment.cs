using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("AttitudeAttchment")]

    
    public class AttitudeAttchment
    {
        

        public int ID { get; set; }
        public string Path { get; set; }
        public int CampaignID { get; set; }
        public int HeadingID { get; set; }
        public int UploadFrom { get; set; }
        public int UploadTo { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? UploadTime { get; set; }

    }

}
