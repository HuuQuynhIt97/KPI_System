﻿using Microsoft.AspNetCore.Http;
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
    public class AttitudeHeadingController : ApiControllerBase
    {
        private readonly IAttitudeHeadingService _service;

        public AttitudeHeadingController(IAttitudeHeadingService service)
        {
            _service = service;
        }

         [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

    }
}
