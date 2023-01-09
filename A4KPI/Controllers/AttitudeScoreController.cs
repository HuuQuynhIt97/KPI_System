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
    public class AttitudeScoreController : ApiControllerBase
    {
        private readonly IAttitudeScoreService _service;
        private readonly IHubContext<KPIHub> _hubContext;
        public AttitudeScoreController(
            IAttitudeScoreService service,
            IHubContext<KPIHub> hubContext
            )
        {
            _service = service;
            _hubContext = hubContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllScoreStation()
        {
            return Ok(await _service.GetAllScoreStation());
        }

        [HttpGet("{from}/{to}")]
        public async Task<IActionResult> GetPoint(string from, string to)
        {
            return Ok(await _service.GetPoint(from, to));
        }

        [HttpGet("{campaignID}/{userFrom}/{userTo}/{type}")]
        public async Task<IActionResult> GetAllAsync(int campaignID, int userFrom, int userTo, string type)
        {
            return Ok(await _service.GetAllAsync(campaignID , userFrom, userTo, type));
        }

        [HttpGet("{campaignID}/{userID}/{type}")]
        public async Task<IActionResult> GetKPISelfScoreDefault(int campaignID, int userID, string type)
        {
            return Ok(await _service.GetKPISelfScoreDefault(campaignID, userID, type));
        }

        [HttpGet("{campaignID}/{userID}/{type}")]
        public async Task<IActionResult> GetKPISelfScoreString(int campaignID, int userID, string type)
        {
            return Ok(await _service.GetKPISelfScoreString(campaignID, userID, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetail(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetail(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailPassion(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailPassion(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailAccountbility(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailAccountbility(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailAttention(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailAttention(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailContinuous(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailContinuous(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailEffective(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailEffective(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{flUser}/{l0User}/{l1User}/{l2User}/{type}")]
        public async Task<IActionResult> GetDetailResilience(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            return Ok(await _service.GetDetailResilience(campaignID, flUser, l0User, l1User, l2User, type));
        }

        [HttpGet("{campaignID}/{userFrom}/{userTo}/{type}")]
        public async Task<IActionResult> GetListCheckBehavior(int campaignID, int userFrom, int userTo, string type)
        {
            return Ok(await _service.GetListCheckBehavior(campaignID, userFrom, userTo, type));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AttitudeScoreDto model)
        {
            return Ok(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> SaveScore([FromBody] SaveScoreDto model)
        {
            return StatusCodeResult(await _service.SaveScore(model));
        }

        [HttpPost]
        public async Task<ActionResult> SubmitAttitude([FromBody] SaveScoreDto model)
        {
            var result = await _service.SubmitAttitude(model);
            var result_content = getSignarlRefresh(model.Type, result.SignarlData);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", result_content, "reload");
            return StatusCodeResult(result);
        }

        private object getSignarlRefresh(string type , SignarlLoadUseDto result)
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
        [HttpGet("{campaignId}")]
        public async Task<IActionResult> GetAllByCampaignAsync(int campaignId)
        {
            return Ok(await _service.GetAllByCampaignAsync(campaignId));
        }


        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] AttitudeScoreDto model)
        {
            return Ok(await _service.UpdateAsync(model));

        }
        [HttpPut("{campaignId}/{submitTo}/{currentKPI}/{currentAtt}")]
        public async Task<ActionResult> UpdateAttitudeSubmit(int campaignId, int submitTo, string currentKPI, string currentAtt)
        {
            return Ok(await _service.UpdateAttitudeSubmit(campaignId, submitTo, currentKPI, currentAtt));
        }

        //[HttpPut]
        //public async Task<ActionResult> UpdateReviseStation([FromBody] ReviseStationDto model)
        //{
        //    return Ok(await _service.UpdateReviseStation(model));

        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

    }
}
