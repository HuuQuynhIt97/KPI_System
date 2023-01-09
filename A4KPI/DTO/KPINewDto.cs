using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class KPINewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int Level { get; set; }
        public int? Sequence { get; set; }
        public int? SequenceCHM { get; set; }
        public int? OcId { get; set; }
        public int? PolicyId { get; set; }
        public int TypeId { get; set; }
        public List<int> Pics { get; set; }
        public int Pic { get; set; }
        public bool Submitted { get; set; }
        public bool IsDisplayTodo { get; set; }
        public int? ParentId { get; set; }
        public int UpdateBy { get; set; }
        public int CreateBy { get; set; }
        public int? LevelOcCreateBy { get; set; }
        public int? LevelOcPolicy { get; set; }
        public int? OcIdPolicy { get; set; }
        public int? OcIdCreateBy { get; set; }
        public string PolicyName { get; set; }
        public string TypeName { get; set; }
        public string TypeText { get; set; }
        public string PICName { get; set; }
        public string UpdateName { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdateDate { get; set; }
        public string CreatedTimeYear { get; set; }

        public string StartDisplayMeetingTime { get; set; }

        public string EndDisplayMeetingTime { get; set; }

        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }

        public string FactName { get; set; }
        public string CenterName { get; set; }
        public string DeptName { get; set; }
        public string Mgmt { get; set; }
        public string NameParent { get; set; }
        //public bool IsManager { get; set; }

        public List<int> KpiIds { get; set; }
        public List<KPIAccount> KPIsss { get; set; }

    }
}