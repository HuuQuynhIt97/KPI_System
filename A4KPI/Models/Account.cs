using A4KPI.Models.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A4KPI.Models
{
    [Table("Accounts")]
    public class Account: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(255)]
        public string FullName { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        public bool IsLock { get; set; }
        public int? AccountTypeId { get; set; }
        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }
        public int? Leader { get; set; }
        public int? Manager { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
        public bool? L0 { get; set; }
        public int? L1 { get; set; }
        public int? L2 { get; set; }
        public int? FunctionalLeader  { get; set; }
        public bool? GHR { get; set; }
        public int? JobTitleId { get; set; }
        public bool? GM { get; set; }
        public bool? GMScore { get; set; }
        public int? SystemFlow { get; set; }

        [ForeignKey(nameof(AccountTypeId))]
        public virtual AccountType AccountType { get; set; }

        public virtual ICollection<AccountGroupAccount> AccountGroupAccount { get; set; }


    }
}
