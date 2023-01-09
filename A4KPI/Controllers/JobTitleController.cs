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
    public class JobTitleController : ApiControllerBase
    {
        private readonly IJobTitleService _service;

        public JobTitleController(IJobTitleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("{lang}")]
        public async Task<IActionResult> GetAllByLangAsync(string lang)
        {
            return Ok(await _service.GetAllByLangAsync(lang));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] JobTitleDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

       

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] JobTitleDto model)
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
