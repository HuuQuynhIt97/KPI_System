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
    //[ApiController]
    //[Route("api/[controller]/[action]")]
    public class OCController : ApiControllerBase
    {
        private readonly IOCService _service;

        public OCController(IOCService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllLevel3()
        {
            var oc = await _service.GetAllLevel3();
            return Ok(oc);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsTreeView()
        {
            var ocs = await _service.GetAllAsTreeView();
            return Ok(ocs);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] OCDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

       

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] OCDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }


    }
}
