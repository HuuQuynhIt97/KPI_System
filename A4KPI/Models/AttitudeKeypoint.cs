using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("AttitudeKeypoint")]
    public class AttitudeKeypoint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AttitudeCategoryID { get; set; }
        public int AttitudeHeadingID { get; set; }
        public int Level { get; set; }

    }
}
