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
    public class VersionController : ApiControllerBase
    {
        private readonly IVersionService _service;

        public VersionController(IVersionService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetAllAsync();
            return Ok(data.FirstOrDefault(x => x.ID == id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _service.GetAllAsync();
            return Ok(plans);
        }
        [HttpGet("{text}")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, string text)
        {
            var lists = await _service.Search(param, text);
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Version create)
        {

            if (_service.GetById(create.ID) != null)
                return BadRequest("Version already exists!");
            create.CreatedTime = DateTime.Now;
            if (await _service.Add(create))
            {
                return NoContent();
            }

            throw new Exception("Creating the Version failed on save");
        }
        [HttpPut]
        public async Task<IActionResult> Update(Models.Version update)
        {
            update.UpdatedTime = DateTime.Now;
            if (await _service.Update(update))
                return NoContent();
            return BadRequest($"Updating the Version {update.ID} failed on save");
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
