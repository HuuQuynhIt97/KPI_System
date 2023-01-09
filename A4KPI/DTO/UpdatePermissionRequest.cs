using System.Collections.Generic;
using A4KPI.DTO;

namespace A4KPI.DTO
{
    public class UpdatePermissionRequest
    {
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
