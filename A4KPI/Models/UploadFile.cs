using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("UploadFile")]
    public class UploadFile : IDateTracking
    {
        public UploadFile()
        {
        }

        public UploadFile(string path, int kPIId, DateTime uploadTime)
        {
            Path = path;
            KPIId = kPIId;
            UploadTime = uploadTime;
        }

        

        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public int KPIId { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
