using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Threading;
using A4KPI._Services.Interface;

namespace A4KPI.Controllers
{
    public class HQReportController : ApiControllerBase
    {
        private readonly IHQReportService _service;

        public HQReportController(IHQReportService service)
        {
            _service = service;
        }

        // [HttpGet]
        // public async Task<ActionResult> GetAllAsync(string lang)
        // {
        //     return Ok(await _service.GetAllAsync(lang));
        // }
        // [HttpGet("{campaignID}/{userID}")]
        // public async Task<IActionResult> GetKPIDefaultPerson(int campaignID, int userID)
        // {
        //     return Ok(await _service.GetKPIDefaultPerson(campaignID, userID));
        // }

        // [HttpGet("{campaignID}/{userID}")]
        // public async Task<IActionResult> GetKPIStringPerson(int campaignID, int userID)
        // {
        //     return Ok(await _service.GetKPIStringPerson(campaignID, userID));
        // }

        // [HttpGet("{campaignID}/{userID}")]
        // public async Task<IActionResult> GetKPIDefaultMuti(int campaignID, int userID)
        // {
        //     return Ok(await _service.GetKPIDefaultMuti(campaignID, userID));
        // }

        // [HttpGet("{campaignID}/{userID}")]
        // public async Task<IActionResult> GetKPIStringMuti(int campaignID, int userID)
        // {
        //     return Ok(await _service.GetKPIStringMuti(campaignID, userID));
        // }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<ActionResult> GetAllHQReport(string lang, int campaignID)
        {
            return Ok(await _service.GetAllHQReport(lang, campaignID));
        }

        [HttpGet("{campaignID}")]
        public async Task<string> GetTitleH1HQReport(int campaignID)
        {
            return await _service.GetTitleH1HQReport(campaignID);
        }
        
        // [HttpGet("{appraiseeID}")]
        // public async Task<ActionResult> GetPeopleCommittee(int appraiseeID)
        // {
        //     return Ok(await _service.GetPeopleCommittee(appraiseeID));
        // }

        // [HttpGet("{appraiseeID}")]
        // public async Task<ActionResult> GetKpi(int appraiseeID)
        // {
        //     return Ok(await _service.GetKpi(appraiseeID));
        // }

        // [HttpGet("{scoreTo}/{campaignID}")]
        // public async Task<ActionResult> GetAllKpiScore(int scoreTo, int campaignID)
        // {
        //     return Ok(await _service.GetAllKpiScore(scoreTo, campaignID));
        // }

        // [HttpGet("{scoreTo}/{campaignID}")]
        // public async Task<ActionResult> GetAllAttitudeScore(int scoreTo, int campaignID)
        // {
        //     return Ok(await _service.GetAllAttitudeScore(scoreTo, campaignID));
        // }

        // [HttpGet("{scoreTo}/{campaignID}")]
        // public async Task<ActionResult> GetSumAttitudeScore(int scoreTo, int campaignID)
        // {
        //     return Ok(await _service.GetSumAttitudeScore(scoreTo, campaignID));
        // }

        // [HttpGet("{scoreTo}/{campaignID}")]
        // public async Task<ActionResult> GetSpecialScoreDetail(int scoreTo, int campaignID)
        // {
        //     return Ok(await _service.GetSpecialScoreDetail(scoreTo, campaignID));
        // }

        // [HttpGet("{scoreTo}/{campaignID}")]
        // public async Task<ActionResult> GetScoreL2(int scoreTo, int campaignID)
        // {
        //     return Ok(await _service.GetScoreL2(scoreTo, campaignID));
        // }

        // [HttpGet("{scoreTo}/{scoreFrom}/{campaignID}")]
        // public async Task<ActionResult> GetCommitteeScore(int scoreTo, int scoreFrom, int campaignID)
        // {
        //     return Ok(await _service.GetCommitteeScore(scoreTo, scoreFrom, campaignID));
        // }

        // [HttpGet("{campaignID}")]
        // public async Task<bool> GetFrozen(int campaignID)
        // {
        //     return await _service.GetFrozen(campaignID);
        // }  

        // [HttpPut]
        // public async Task<ActionResult> UpdateKpiScore([FromBody] KPIScoreDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateKpiScore(model));
        // }

        // [HttpPut]
        // public async Task<ActionResult> UpdateAttitudeScore([FromBody] PeopleCommitteeAttScoreDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateAttitudeScore(model));
        // }

        // [HttpPut]
        // public async Task<ActionResult> UpdateSpecialScore([FromBody] SpecialContributionScoreDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateSpecialScore(model));
        // }
        // [HttpPut]
        // public async Task<ActionResult> UpdateKpiScoreL2([FromBody] PeopleCommitteeSpecialScoreDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateKpiScoreL2(model));
        // }
        // [HttpPut]
        // public async Task<ActionResult> UpdateCommitteeScore([FromBody] CommitteeScoreDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateCommitteeScore(model));
        // }
        // [HttpPut]
        // public async Task<ActionResult> UpdateCommitteeSequence([FromBody] CommitteeSequenceDto model)
        // {
        //     return StatusCodeResult(await _service.UpdateCommitteeSequence(model));
        // }
        // [HttpPut("{campaignID}")]
        // public async Task<ActionResult> LockUpdate(int campaignID)
        // {
        //     return StatusCodeResult(await _service.LockUpdate(campaignID));
        // }
    }
}
