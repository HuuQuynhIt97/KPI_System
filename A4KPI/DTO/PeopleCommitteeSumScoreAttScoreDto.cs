using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class PeopleCommitteeSumScoreAttScoreDto
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public double? SumPoint { get; set; }
        public object Files { get; set; }
        public string ScoreFromName { get; set; }
    }
}
