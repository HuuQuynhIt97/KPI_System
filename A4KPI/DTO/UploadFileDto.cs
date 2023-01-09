using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class UploadFileDto
    {
    }
    public class PostRequest
    {
        public int KpiId { get; set; }
        public DateTime CurrentTime { get; set; }
    }
    public class PreloadFileDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public object Size{ get; set; }
        public object Type{ get; set; }
    }
    public class DownloadFileDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
