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
using Microsoft.AspNetCore.SignalR;
using A4KPI.SignalR;

namespace A4KPI.Controllers
{
    public class KPIScoreController : ApiControllerBase
    {
        private readonly IKPIScoreService _service;
        private readonly IHubContext<KPIHub> _hubContext;
        public KPIScoreController(
            IKPIScoreService service,
            IHubContext<KPIHub> hubContext
            )
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{campaignID}/{userID}/{type}")]
        public async Task<IActionResult> GetKPIScoreDetail(int campaignID, int userID, string type)
        {
            return Ok(await _service.GetKPIScoreDetail(campaignID, userID, type));
        }

        [HttpGet("{campaignID}/{userID}/{type}")]
        public async Task<IActionResult> GetKPIScoreL2L1Detail(int campaignID, int userID, string type)
        {
            return Ok(await _service.GetKPIScoreL2L1Detail(campaignID, userID, type));
        }

        [HttpGet("{campaignID}/{scoreFrom}/{scoreTo}/{type}")]
        public async Task<IActionResult> GetKPIScoreL1L0Detail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            return Ok(await _service.GetKPIScoreL1L0Detail(campaignID, scoreFrom, scoreTo, type));
        }

        [HttpGet("{campaignID}/{scoreFrom}/{scoreTo}/{type}")]
        public async Task<IActionResult> GetKPIScoreGMDetail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            return Ok(await _service.GetKPIScoreGMDetail(campaignID, scoreFrom, scoreTo, type));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] KPIScoreDto model)
        {
            var result = await _service.AddAsync(model);
            var result_content = getSignarlRefresh(model.ScoreType, result.SignarlData);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", result_content, "reload");
            return StatusCodeResult(result);
        }
        [HttpPost]
        public async Task<ActionResult> SubmitKPI([FromBody] KPIScoreDto model)
        {
            var result = await _service.SubmitKPI(model);
            var result_content = getSignarlRefresh(model.ScoreType, result.SignarlData);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", result_content, "reload");
            return StatusCodeResult(result);
        }
        private object getSignarlRefresh(string type, SignarlLoadUseDto result)
        {
            var result_content = new object();
            switch (type.ToUpper())
            {
                case "FL":
                    result_content = new
                    {
                        loadUserFrom = result.L0,
                        loadUserTo = result.FL
                    };
                    break;
                case "L0":
                    result_content = new
                    {
                        loadUserFrom = result.L0,
                        loadUserTo = result.L1
                    };
                    break;
                case "L1":
                    result_content = new
                    {
                        loadUserFrom = result.L1,
                        loadUserTo = result.L2
                    };
                    break;
                case "L2":
                    result_content = new
                    {
                        loadUserFrom = result.L2,
                        loadUserTo = result.L2
                    };
                    break;
                default:
                    break;
            }
            return result_content;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] KPIScoreDto model)
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
