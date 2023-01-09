using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class PeopleCommitteeAttScoreDto
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
        public string Point { get; set; }
        public string NameAttitudeHeading { get; set; }
        public string ScoreFromName { get; set; }
        public object Files { get; set; }
    }
}
