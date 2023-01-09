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
    public class AttitudeKeypointController : ApiControllerBase
    {
        private readonly IAttitudeKeypointService _service;

        public AttitudeKeypointController(IAttitudeKeypointService service)
        {
            _service = service;
        }

        [HttpGet("{attitudeHeadingID}/{campaignID}")]
        public async Task<IActionResult> GetAllByAttitudeScore(int attitudeHeadingID, int campaignID)
        {
            return Ok(await _service.GetAllByAttitudeScore(attitudeHeadingID, campaignID));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AttitudeCategoryDto attitudeCategory)
        {
            return Ok(await _service.Add(attitudeCategory));
        }

    }
}
