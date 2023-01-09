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
    public class ReportController : ApiControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }
        [HttpGet("{userId}/{campaignID}")]
        public async Task<ActionResult> TrackingAppaisalProgress(int userId, int campaignID)
        {

            return Ok(await _service.TrackingAppaisalProgress(userId, campaignID));
        }

        [HttpGet("{currentTime}/{userId}")]
        public async Task<ActionResult> TrackingProcess(DateTime currentTime, int userId)
        {

            return Ok(await _service.ReportPDCA(currentTime , userId));
        }

        [HttpGet("{currentTime}/{userId}")]
        public async Task<ActionResult> TrackingProcessKPI(DateTime currentTime, int userId)
        {

            return Ok(await _service.ReportPDCA2(currentTime, userId));
        }
    }
}
