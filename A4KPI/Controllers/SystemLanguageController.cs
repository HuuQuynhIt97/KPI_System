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
    public class SystemLanguageController : ApiControllerBase
    {
        private readonly ISystemLanguageService _service;

        public SystemLanguageController(ISystemLanguageService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateLanguage()
        {
            
            var lists = await _service.UpdateLanguage();
            return Ok(lists);
           

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _service.GetAllAsync();
            return Ok(plans);
        }
        [HttpGet("{lang}")]
        public async Task<IActionResult> GetLanguages(string lang)
        {
            var plans = await _service.GetLanguages(lang);
            return Ok(plans);
        }
        [HttpGet("{text}")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, string text)
        {
            var lists = await _service.Search(param, text);
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.SystemLanguage create)
        {
            return Ok(await _service.Add(create));
            // if (await _service.Add(create))
            // {
            //     return NoContent();
            // }

            // throw new Exception("Creating the System language failed on save");
        }
        [HttpPut]
        public async Task<IActionResult> Update(Models.SystemLanguage update)
        {
            return Ok(await _service.Update(update));
            // if (await _service.Update(update))
            //     return NoContent();
            // return BadRequest($"Updating the Version {update.ID} failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.Delete(id))
                return NoContent();
            throw new Exception("Error deleting the Version");
        }

    }
}
