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
    public class CoreCompetenciesController : ApiControllerBase
    {
        private readonly ICoreCompetenciesService _service;

        public CoreCompetenciesController(ICoreCompetenciesService service)
        {
            _service = service;
        }

        //New-Attitude

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllNewCoreCompetencies(string lang, int campaignID)
        {
            return Ok(await _service.GetAllNewCoreCompetencies(lang, campaignID));
        }
        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetNewCoreCompetencies(string lang, int campaignID)
        {
            return Ok(await _service.GetNewCoreCompetencies(lang, campaignID));
        }
        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetNewCoreCompetenciesScoreEquals2(string lang, int campaignID)
        {
            return Ok(await _service.GetNewCoreCompetenciesScoreEquals2(lang, campaignID));
        }
        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetNewCoreCompetenciesScoreThan2(string lang, int campaignID)
        {
            return Ok(await _service.GetNewCoreCompetenciesScoreThan2(lang, campaignID));
        }
        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetNewCoreCompetenciesAverage(string lang, int campaignID)
        {
            return Ok(await _service.GetNewCoreCompetenciesAverage(lang, campaignID));
        }
        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetNewCoreCompetenciesPercentile(string lang, int campaignID)
        {
            return Ok(await _service.GetNewCoreCompetenciesPercentile(lang, campaignID));
        }
        [HttpPost("{lang}/{campaignID}")]
        public async Task<IActionResult> ExportExcelNewCoreCompetencies(string lang, int campaignID)
        {
            var bin = await _service.ExportExcelNewCoreCompetencies(lang, campaignID);
            return File(bin, "application/octet-stream", "CoreCompetencies.xlsx");
        }
        ///
        

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetencies(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetencies(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesScoreEquals2(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesScoreEquals2(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesScoreThan2(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesScoreThan2(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesScoreThan3(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesScoreThan3(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesAverage(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesAverage(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesPercentile(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesPercentile(lang, campaignID));
        }

        [HttpGet("{lang}/{campaignID}")]
        public async Task<IActionResult> GetAllCoreCompetenciesAttitudeBehavior(string lang, int campaignID)
        {
            return Ok(await _service.GetAllCoreCompetenciesAttitudeBehavior(lang, campaignID));
        }

        [HttpPost("{lang}/{campaignID}")]
        public async Task<IActionResult> ExportExcelCoreCompetencies(string lang, int campaignID)
        {
            var bin = await _service.ExportExcelCoreCompetencies(lang, campaignID);
            return File(bin, "application/octet-stream", "CoreCompetencies.xlsx");
        }
    }
}
