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
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(string lang)
        {
            return Ok(await _service.GetAllAsync(lang));
        }
        [HttpGet("{lang}")]
        public async Task<ActionResult> GetAllByLangAsync(string lang)
        {
            return Ok(await _service.GetAllAsync(lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            return Ok(await _service.GetAccounts());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AccountDto model)
        {
            return Ok(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] AccountDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> LockAsync(int id)
        {
            return StatusCodeResult(await _service.LockAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateL0Async(int id)
        {
            return StatusCodeResult(await _service.UpdateL0Async(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateGhrAsync(int id)
        {
            return StatusCodeResult(await _service.UpdateGhrAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateGmAsync(int id)
        {
            return StatusCodeResult(await _service.UpdateGmAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateGmScoreAsync(int id)
        {
            return StatusCodeResult(await _service.UpdateGmScoreAsync(id));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }


        [HttpPut]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            return StatusCodeResult(await _service.ChangePasswordAsync(request));
            //return Ok(await _service.ChangePasswordAsync(request));
        }

    }
}
