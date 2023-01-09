using System.Collections.Generic;
using A4KPI.DTO;

namespace A4KPI.DTO
{
    public class UpdateActionInFunctionRequest
    {
        public List<ActionInFunctionSystemDto> ActionInFunction { get; set; } = new List<ActionInFunctionSystemDto>();
    }
}
