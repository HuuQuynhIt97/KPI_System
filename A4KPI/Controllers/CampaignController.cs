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
    public class CampaignController : ApiControllerBase
    {
        private readonly ICampaignService _service;

        public CampaignController(ICampaignService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CampaignDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost("{campaignID}")]
        public async Task<ActionResult> GenerateEvaluation(int campaignID)
        {
            return StatusCodeResult(await _service.GenerateEvaluation(campaignID));
        }
        
        [HttpPost("{campaignID}")]
        public async Task<ActionResult> GenerateAttitude(int campaignID)
        {
            return StatusCodeResult(await _service.GenerateAttitude(campaignID));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] CampaignDto model)
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
