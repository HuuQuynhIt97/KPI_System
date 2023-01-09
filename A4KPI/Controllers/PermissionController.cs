using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Services.Interface;

namespace A4KPI.Controllers
{
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service)
        {
            _service = service;
        }
        [HttpGet("{roleID}")]
        public async Task<IActionResult> GetActionInFunctionByRoleID(int roleID)
        {
            var result = await _service.GetActionInFunctionByRoleID(roleID);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuByLangID([FromQuery] int userID, [FromQuery] string langID)
        {
            //create new permission list from user changed

            var result = await _service.GetMenuByLangID(userID, langID);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request)
        {
            //create new permission list from user changed

            var result = await _service.GetScreenFunctionAndAction(request);
            return Ok(result);
        }
        [HttpPut("{roleId}")]
        public async Task<IActionResult> PutPermissionByRoleId(int roleId, [FromBody] UpdatePermissionRequest request)
        {
            //create new permission list from user changed

            var result = await _service.PutPermissionByRoleId(roleId, request);
            if (result.Status)
            {
                return NoContent();
            }
            return BadRequest("Save permission failed");
        }


    }
}
