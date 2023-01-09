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
    //[ApiController]
    //[Route("api/[controller]/[action]")]
    public class NewAttitudeScoreController : ApiControllerBase
    {
        private readonly INewAttitudeScoreService _service;
        private readonly IHubContext<KPIHub> _hubContext;

        public NewAttitudeScoreController(INewAttitudeScoreService service, IHubContext<KPIHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpGet("{campaignID}/{scoreTo}/{scoreFrom}")]
        public async Task<IActionResult> GetAllAsync(int campaignID, int scoreTo, int scoreFrom)
        {
            return Ok(await _service.GetAllAsync(campaignID, scoreTo, scoreFrom));
        }

        [HttpPut("{id}/{point}")]
        public async Task<ActionResult> UpdatePointAsync(int id, string point)
        {
            return StatusCodeResult(await _service.UpdatePointAsync(id, point));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCommentAsync([FromBody] NewAttitudeAttchmentDto model)
        {
            return StatusCodeResult(await _service.UpdateCommentAsync(model));
        }

        [HttpGet("{campaignID}/{scoreTo}/{scoreFrom}")]
        public async Task<IActionResult> GetAttEvaluationAsync(int campaignID, int scoreTo, int scoreFrom)
        {
            return Ok(await _service.GetAttEvaluationAsync(campaignID, scoreTo, scoreFrom));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAttEvaluationAsync([FromBody] NewAttitudeEvaluationDto model)
        {
            return StatusCodeResult(await _service.UpdateAttEvaluationAsync(model));
        }

        [HttpGet("{campaignID}/{scoreTo}")]
        public async Task<IActionResult> GetNewAttitudeSubmit(int campaignID, int scoreTo)
        {
            return Ok(await _service.GetNewAttitudeSubmit(campaignID, scoreTo));
        }

        [HttpPut("{campaignID}/{scoreTo}/{scoreFrom}/{type}")]
        public async Task<ActionResult> CheckSubmitNewAtt(int campaignID, int scoreTo, int scoreFrom, string type)
        {
            var result = await _service.CheckSubmitNewAtt(campaignID, scoreTo, scoreFrom, type);
            var result_content = getSignarlRefresh(type, result.SignarlData);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", result_content, "reload");
            return Ok(result);
        }

        private object getSignarlRefresh(string type , SignarlLoadUseDto result)
        {
            var result_content = new object();
            if (result == null)
            {
                return result_content = new
                    {
                        loadUserFrom = 0,
                        loadUserTo = 0
                    };
            }
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

        [HttpPut("{campaignID}/{scoreTo}/{scoreFrom}")]
        public async Task<ActionResult> GenerateNewAttitudeScore(int campaignID, int scoreTo, int scoreFrom)
        {
            return StatusCodeResult(await _service.GenerateNewAttitudeScore(campaignID, scoreTo, scoreFrom));
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

        [HttpGet("{campaignID}/{scoreTo}")]
        public async Task<IActionResult> GetDetailNewAttitude(int campaignID, int scoreTo)
        {
            return Ok(await _service.GetDetailNewAttitude(campaignID, scoreTo));
        }
        
        [HttpGet("{campaignID}/{scoreTo}")]
        public async Task<IActionResult> GetDetailNewAttitudeEvaluation(int campaignID, int scoreTo)
        {
            return Ok(await _service.GetDetailNewAttitudeEvaluation(campaignID, scoreTo));
        }

    }
}
