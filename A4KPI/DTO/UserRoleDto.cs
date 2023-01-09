using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class UserRoleDto
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public bool IsLock { get; set; }
    }
}
