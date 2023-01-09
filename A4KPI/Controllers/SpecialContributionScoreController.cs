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
    public class SpecialContributionScoreController : ApiControllerBase
    {
        private readonly ISpecialContributionScoreService _service;

        public SpecialContributionScoreController(ISpecialContributionScoreService service)
        {
            _service = service;
        }


        [HttpGet("{campaignID}/{scoreTo}/{type}")]
        public async Task<ActionResult> GetMultiType(int campaignID, int scoreTo, string type)
        {
            return Ok(await _service.GetMultiType(campaignID, scoreTo, type));
        }

        [HttpGet("{campaignID}/{scoreTo}/{type}")]
        public async Task<ActionResult> GetMultiImpact(int campaignID, int scoreTo, string type)
        {
            return Ok(await _service.GetMultiImpact(campaignID,  scoreTo, type));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{lang}")]
        public async Task<IActionResult> GetSpecialType(string lang)
        {
            return Ok(await _service.GetSpecialType(lang));
        }

        [HttpGet("{lang}")]
        public async Task<IActionResult> GetSpecialCompact(string lang)
        {
            return Ok(await _service.GetSpecialCompact(lang));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialRatio()
        {
            return Ok(await _service.GetSpecialRatio());
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialScore()
        {
            return Ok(await _service.GetSpecialScore());
        }


        [HttpGet("{campaignID}/{scoreFrom}/{scoreTo}/{type}")]
        public async Task<IActionResult> GetSpecialScoreDetail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            return Ok(await _service.GetSpecialScoreDetail(campaignID, scoreFrom, scoreTo, type));
        }

        [HttpGet("{campaignID}/{userID}/{type}")]
        public async Task<IActionResult> GetSpecialL1ScoreDetail(int campaignID, int userID, string type)
        {
            return Ok(await _service.GetSpecialL1ScoreDetail(campaignID, userID, type));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] SpecialContributionScoreDto model)
        {
            return Ok(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] SpecialContributionScoreDto model)
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
