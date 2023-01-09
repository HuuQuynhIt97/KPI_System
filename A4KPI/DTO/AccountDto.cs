using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Level { get; set; }
        public bool IsLock { get; set; }
        public int? AccountTypeId { get; set; }
        public int? Leader { get; set; }
        public string LeaderName { get; set; }
        public string FactName { get; set; }
        public string CenterName { get; set; }
        public string DeptName { get; set; }
        public string Role { get; set; }
        public string RoleCode { get; set; }
        public int? Manager { get; set; }
        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }
        public string ManagerName { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public List<int> AccountGroupIds { get; set; }
        public string AccountGroupText { get; set; }
        public bool? L0 { get; set; }
        public int? L1 { get; set; }
        public string L1Name { get; set; }
        public int? L2 { get; set; }
        public string L2Name { get; set; }
        public int? FunctionalLeader  { get; set; }
        public string FunctionalLeaderName  { get; set; }
        public bool? GHR { get; set; }
        public int? JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public bool? GM { get; set; }
        public bool? GMScore { get; set; }
        public int? SystemFlow { get; set; }
    }
    public class ChangePasswordRequest
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
