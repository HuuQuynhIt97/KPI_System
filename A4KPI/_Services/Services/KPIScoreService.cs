using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;
using A4KPI.Helpers;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using A4KPI.Constants;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
   
    public class KPIScoreService : IKPIScoreService
    {
        private OperationResult operationResult;
        private readonly IKPIScoreRepository _repo;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly IAccountRepository _repoAc;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public KPIScoreService(
            IKPIScoreRepository repo,
            IAccountRepository repoAc,
            IAttitudeSubmitRepository repoAttSubmit,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAc = repoAc;
            _repoAttSubmit = repoAttSubmit;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<KPIScore> GetKPIScoreDetail(int campaignID, int userID, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreBy == userID 
                && x.ScoreType == type
            ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<KPIScore> GetKPIScoreL2L1Detail(int campaignID, int userID, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreTo == userID
                && x.ScoreType == type
            ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<KPIScore> GetKPIScoreL1L0Detail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreFrom == scoreFrom
                && x.ScoreTo == scoreTo
                && x.ScoreType == type
            ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<KPIScore> GetKPIScoreGMDetail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreFrom == scoreFrom
                && x.ScoreTo == scoreTo
                && x.ScoreType == type
            ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<OperationResult> AddAsync(KPIScoreDto model)
        {
            var item = _repo.FindAll(
                x => x.CampaignID == model.CampaignID
                && x.ScoreFrom == model.ScoreFrom
                && x.ScoreTo == model.ScoreTo
                && x.ScoreType == model.ScoreType
                ).FirstOrDefault();
            if (item == null)
            {
                var add = _mapper.Map<KPIScore>(model);
                add.ScoreTime = DateTime.Now;
                add.CreatedTime = DateTime.Now;
                _repo.Add(add);
            }else
            {
                item.IsSubmit = model.IsSubmit;
                item.Point = model.Point;
                item.Comment = model.Comment;
                _repo.Update(item);
            }

            // UPDATE ATTITUDESUBMIT_TABLE
            var att_submit = _repoAttSubmit.FindAll(x => x.SubmitTo == model.ScoreTo && x.CampaignID == model.CampaignID).FirstOrDefault();
            var systemFlow_user = _repoAc.FindAll(x => x.Id == model.ScoreTo).FirstOrDefault();

            if (model.IsSubmit)
            {
                switch (model.ScoreType.ToUpper())
                {
                    case "L0":
                        if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                        {
                            att_submit.L0KPI = true;
                            att_submit.BtnL0KPI = false;
                            att_submit.BtnL2KPI = true;
                        }
                        else if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                        {
                            att_submit.L0KPI = true;
                            att_submit.BtnL0KPI = false;
                            att_submit.BtnL1KPI = true;
                        }
                        else
                        {
                            att_submit.L0KPI = true;
                            att_submit.BtnL0KPI = false;
                            att_submit.BtnL1KPI = true;
                        }
                        break;
                    case "L1":
                        if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                        {
                            att_submit.L1KPI = true;
                            att_submit.BtnL1KPI = false;
                            att_submit.BtnL2KPI = true;
                        }
                        else if (systemFlow_user.SystemFlow == 3 || systemFlow_user.SystemFlow == 6)
                        {
                            att_submit.L1KPI = true;
                            att_submit.BtnL1KPI = false;
                            att_submit.BtnL2KPI = true;
                        }

                        break;
                    case "L2":
                        att_submit.L2KPI = true;
                        att_submit.BtnL2KPI = false;
                        break;
                    case "FL":
                        att_submit.FLKPI = true;
                        att_submit.BtnFLKPI = false;
                        att_submit.BtnL0KPI = true;
                        break;
                }
            }
            var infor_user = _repoAc.FindById(model.ScoreTo);
            var result_message = new SignarlLoadUseDto()
            {
                L0 = infor_user.Id,
                L1 = infor_user.Manager.Value,
                L2 = infor_user.L2.Value,
                FL = infor_user.FunctionalLeader.Value
            };
           

            try
            {
                _repoAttSubmit.Update(att_submit);
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    SignarlData = result_message
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var delete = _repo.FindById(id);
            _repo.Remove(delete);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = delete
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<List<KPIScoreDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<KPIScoreDto>(_configMapper)
                .OrderByDescending(x => x.ID).ToListAsync();
        }

        public async Task<OperationResult> UpdateAsync(KPIScoreDto model)
        {
            var update = _mapper.Map<KPIScore>(model);
            _repo.Update(update);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = update
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> SubmitKPI(KPIScoreDto model)
        {
            // UPDATE ATTITUDESUBMIT_TABLE
            var att_submit = _repoAttSubmit.FindAll(x => x.SubmitTo == model.ScoreTo && x.CampaignID == model.CampaignID).FirstOrDefault();
            var systemFlow_user = _repoAc.FindAll(x => x.Id == model.ScoreTo).FirstOrDefault();
            switch (model.ScoreType.ToUpper())
            {
                case "L0":
                    if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                    {
                        att_submit.L0KPI = true;
                        att_submit.BtnL0KPI = false;
                        att_submit.BtnL2KPI = true;
                    }
                    else if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        att_submit.L0KPI = true;
                        att_submit.BtnL0KPI = false;
                        att_submit.BtnL1KPI = true;
                    }
                    else
                    {
                        att_submit.L0KPI = true;
                        att_submit.BtnL0KPI = false;
                        att_submit.BtnL1KPI = true;
                    }
                    break;
                case "L1":
                    if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        att_submit.L1KPI = true;
                        att_submit.BtnL1KPI = false;
                        att_submit.BtnL2KPI = true;
                    }
                    else if (systemFlow_user.SystemFlow == 3 || systemFlow_user.SystemFlow == 6)
                    {
                        att_submit.L1KPI = true;
                        att_submit.BtnL1KPI = false;
                        att_submit.BtnL2KPI = true;
                    }
                    

                    break;
                case "L2":
                    att_submit.L2KPI = true;
                    att_submit.BtnL2KPI = false;
                    break;
                case "FL":
                    att_submit.FLKPI = true;
                    att_submit.BtnFLKPI = false;
                    att_submit.BtnL0KPI = true;
                    break;
            }
            var infor_user = _repoAc.FindById(model.ScoreTo);
            var result_message = new SignarlLoadUseDto()
            {
                L0 = infor_user.Id,
                L1 = infor_user.Manager.Value,
                L2 = infor_user.L2.Value,
                FL = infor_user.FunctionalLeader.Value
            };
            try
            {
                await _repoAttSubmit.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    SignarlData = result_message
                };
            }
            catch (Exception ex)
            {

                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
    }
}
