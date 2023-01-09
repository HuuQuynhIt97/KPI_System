using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class MeetingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int Level { get; set; }
        public int? Sequence { get; set; }
        public int? SequenceCHM { get; set; }
        public int TypeId { get; set; }
        public int? ParentId { get; set; }
        public string TypeName { get; set; }
        public string TypeText { get; set; }
        public string PICName { get; set; }
        public List<int> PICNum { get; set; }
        public List<MeetingDto> result { get; set; }
        public DateTime CreatedTime { get; set; }
        public string FactName { get; set; }
        public string CenterName { get; set; }
        public string DeptName { get; set; }

        public bool Status { get; set; }

    }

    public class MeetingAllDto
    {
        public List<MeetingDto> result { get; set; }
        public string FactName { get; set; }
        public string CenterName { get; set; }
        public string DeptName { get; set; }

        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }
    }
}