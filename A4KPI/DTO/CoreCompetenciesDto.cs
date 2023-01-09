using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class CoreCompetenciesDto
    {
        public int Index { get; set; }
        public int AttHeadingID { get; set; }
        public string AttHeading { get; set; }
        public string Center { get; set; }
        public string Factory { get; set; }
        public string Dept { get; set; }
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string Fl { get; set; }
        public string L0 { get; set; }
        public string SelfScore { get; set; }
        public string Score { get; set; }
        public string ScoreBy { get; set; }
        public string Comment { get; set; }
        public bool Submited { get; set; }
    }
}
