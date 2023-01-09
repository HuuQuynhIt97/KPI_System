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
    public class EvaluationController : ApiControllerBase
    {
        private readonly IEvaluationService _service;

        public EvaluationController(IEvaluationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> GetSelfAppraisal(int userID)
        {
            return Ok(await _service.GetSelfAppraisal(userID));
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> GetFirstLevelAppraisal(int userID)
        {
            return Ok(await _service.GetFirstLevelAppraisal(userID));
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> GetSecondLevelAppraisal(int userID)
        {
            return Ok(await _service.GetSecondLevelAppraisal(userID));
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> GetFLFeedback(int userID)
        {
            return Ok(await _service.GetFLFeedback(userID));
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetGM(int userID)
        {
            return Ok(await _service.GetGMData(userID));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] EvaluationDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost("{campaignID}")]
        public async Task<ActionResult> GenerateEvaluation(int campaignID)
        {
            return StatusCodeResult(await _service.GenerateEvaluation(campaignID));
        }

        [HttpPost("{campaignID}")]
        public async Task<ActionResult> GenerateAttitudeSubmit(int campaignID)
        {
            return StatusCodeResult(await _service.GenerateAttitudeSubmit(campaignID));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] EvaluationDto model)
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
