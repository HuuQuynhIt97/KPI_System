using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("KPIAccount")]
    public class KPIAccount
    {
        //public KPIAccount(int kpiId, int accountId)
        //{
        //    AccountId = accountId;
        //    KpiId = kpiId;
        //}

        [Key]
        public int Id { get; set; }
        public int KpiId { get; set; }
        public int AccountId { get; set; }
        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }
        public bool IsActionSubmit { get; set; }
        public bool IsPDCASubmit { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(KpiId))]
        public virtual KPINew KPINew { get; set; }


        //public virtual ICollection<Action> Actions { get; set; }
        //public virtual OC OCs { get; set; }
    }
}
