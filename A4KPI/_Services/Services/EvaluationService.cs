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
    public class EvaluationService : IEvaluationService
    {
        private OperationResult operationResult;
        private readonly IEvaluationRepository _repo;
        private readonly IEvaluationRepository _repoEval;
        private readonly IAccountRepository _repoAc;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IOCRepository _repoOC;
        private readonly ISystemFlowRepository _repoSystemFlow;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly ICampaignRepository _repoCampaign;
        private readonly IAccountCampaignRepository _repoAcCampaign;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public EvaluationService(
            IEvaluationRepository repo,
            IAttitudeSubmitRepository repoAttSubmit,
            IAccountRepository repoAc,
            ISystemFlowRepository repoSystemFlow,
            IEvaluationRepository repoEval,
            IUserRoleRepository repoUserRole,
            IOCRepository repoOC,
            IRoleRepository repoRole,
            ICampaignRepository repoCampaign,
            IAccountCampaignRepository repoAcCampaign,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAttSubmit = repoAttSubmit;
            _repoSystemFlow = repoSystemFlow;
            _repoAc = repoAc;
            _repoEval = repoEval;
            _repoOC = repoOC;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _repoCampaign = repoCampaign;
            _repoAcCampaign = repoAcCampaign;
            _mapper = mapper;
            _configMapper = configMapper;
        }


        public async Task<OperationResult> GenerateAttitudeSubmit(int campaignID)
        {
            var check_user_generate = await _repoAc.FindAll(x => x.AccountType.Code != Systems.Administrator).ToListAsync();
            var list_add = new List<AttitudeSubmit>();
            var list_addAcCampaign = new List<AccountCampaign>();
            foreach (var item in check_user_generate)
            {
                var role_id = _repoUserRole.FindAll(x => x.UserID == item.Id).FirstOrDefault().RoleID;
                var role_guard = _repoRole.FindById(role_id).Code;
                if (role_guard != SystemRole.SYSTEMADMIN)
                {
                    var systemFlow_user = _repoSystemFlow.FindAll(x => x.SystemFlowID == item.SystemFlow).FirstOrDefault();
                    if (systemFlow_user != null)
                    {
                        if (systemFlow_user.FL)
                        {
                            var item_add = new AttitudeSubmit
                            {
                                SubmitTo = item.Id,
                                IsDisplayFL = systemFlow_user.FL,
                                IsDisplayL0 = systemFlow_user.L0,
                                IsDisplayL1 = systemFlow_user.L1,
                                IsDisplayL2 = systemFlow_user.L2,
                                BtnFL = true,
                                BtnFLKPI = true,
                                BtnNewAttFL = true,
                                CampaignID = campaignID

                            };
                            list_add.Add(item_add);
                        }else
                        {
                            var item_add = new AttitudeSubmit
                            {
                                SubmitTo = item.Id,
                                IsDisplayFL = systemFlow_user.FL,
                                BtnL0 = true,
                                BtnL0KPI = true,
                                BtnNewAttL0 = true,
                                IsDisplayL0 = systemFlow_user.L0,
                                IsDisplayL1 = systemFlow_user.L1,
                                IsDisplayL2 = systemFlow_user.L2,
                                CampaignID = campaignID

                            };
                            list_add.Add(item_add);
                        }

                        var item_addAcCampaign = new AccountCampaign
                            {
                                AccountID = item.Id,
                                L1 = item.Manager.HasValue ? item.Manager.Value : 0,
                                L2 = item.L2.HasValue ? item.L2.Value : 0,
                                FL = item.FunctionalLeader.HasValue ? item.FunctionalLeader.Value : 0,
                                IsL0 = item.L0,
                                CampaignID = campaignID,
                                SystemFlow = item.SystemFlow

                            };
                            list_addAcCampaign.Add(item_addAcCampaign);
                        
                    }
                }

            }

            try
            {
                _repoAttSubmit.AddRange(list_add);
                _repoAcCampaign.AddRange(list_addAcCampaign);
                await _repoAttSubmit.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }


        public async Task<OperationResult> AddAsync(EvaluationDto model)
        {
            var add = _mapper.Map<Evaluation>(model);
            add.CreatedTime = DateTime.Now;
            _repo.Add(add);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = add
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

        public async Task<List<EvaluationDto>> GetAllAsync()
        {
            var result = await _repo.FindAll().Select(x => new EvaluationDto
            {
                //ID =  x.ID,
                //Name =  x.Name,
                //StartMonth = x.StartMonth,
                //EndMonth = x.EndMonth,
                //MonthName = x.MonthName,
                //Year = x.Year,
                //CreatedTime = x.CreatedTime.ToStringDateTime("yyyy-MM-dd"),
                //Creator = _repoAc.FindById(x.CreatedBy).FullName
            }).OrderByDescending(x => x.ID).ToListAsync();
            return result;
        }
        public async Task<OperationResult> UpdateAsync(EvaluationDto model)
        {
            var update = _mapper.Map<Evaluation>(model);
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

        public async Task<OperationResult> GenerateEvaluation(int campaignID)
        {
            //update isStart status
            var update = _repo.FindById(campaignID);
            //check L0 yes-no
            var check_user_generate =  await _repoAc.FindAll().ToListAsync();
            var list_add = new List<Evaluation>();
            foreach (var item in check_user_generate)
            {
                var role_id = _repoUserRole.FindAll(x => x.UserID == item.Id).FirstOrDefault().RoleID;
                var role_guard = _repoRole.FindById(role_id).Code;
                if (role_guard != SystemRole.SYSTEMADMIN)
                {
                    var item_add = new Evaluation
                    {
                        UserID = item.Id
                    };
                    list_add.Add(item_add);
                }
                
            }

            try
            {
                _repo.Update(update);
                _repoEval.AddRange(list_add);
                await _repoEval.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<List<EvaluationDto>> GetSelfAppraisal(int userID)
        {
            var list_evaluation = await _repoEval.FindAll(x => x.UserID == userID).ToListAsync();
            var list_ac = await _repoAc.FindAll(x => x.L0.Value).ToListAsync();

            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID
                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,
                              isDisplayAttitudeBtn = display,
                              isDisplayKPIBtn = displayKPIBtn,
                              Type = "L0",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI
                          }).Where(x => x.isDisplayAttitudeBtn || x.isDisplayKPIBtn).ToList();
            return result;
            
        }

        public async Task<List<EvaluationDto>> GetFirstLevelAppraisal(int userID)
        {
            var list_evaluation = await _repoEval.FindAll().ToListAsync();
            var list_ac = await _repoAc.FindAll(x => x.Manager == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID


                          //attitude
                          let displayFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false

                          let displayL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          //kpi

                          let displayKPIBtnFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFLKPI : false

                          let displayKPIBtnL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                           
                          // disable-enable button
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtn = displayKPIBtn,

                              Type = "L1",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI
                          }).Where(x 
                          => x.isDisplayAttitudeBtnL0 == false 
                          && x.isDisplayAttitudeBtnFL == false
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && (x.isDisplayAttitudeBtn || x.isDisplayKPIBtn)).ToList();
            return result;
            
        }

        public async Task<List<EvaluationDto>> GetSecondLevelAppraisal(int userID)
        {

            var list_evaluation = await _repoEval.FindAll().ToListAsync();
            var isGM = _repoAc.FindById(userID).GM.Value;
            var list_ac = await _repoAc.FindAll(x => x.L2 == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          //attitude
                          let displayFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false

                          let displayL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let displayL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2 : false


                          //kpi

                          let displayKPIBtnFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFLKPI : false

                          let displayKPIBtnL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayKPIBtnL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2KPI : false

                          //disable button kpi - attitude
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2KPI : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtnL1 = displayL1,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtnL1 = displayKPIBtnL1,
                              isDisplayKPIBtn = displayKPIBtn,

                              Type = "L2",
                              isGM = isGM,
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI
                          }).Where(x 
                          => 
                          x.isDisplayAttitudeBtnFL == false 
                          && x.isDisplayAttitudeBtnL0 == false
                          && x.isDisplayAttitudeBtnL1 == false
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && x.isDisplayKPIBtnL1 == false
                          && (x.isDisplayAttitudeBtn || x.isDisplayKPIBtn)).ToList();
            return result;

          
        }

        public async Task<List<EvaluationDto>> GetFLFeedback(int userID)
        {
            var list_evaluation = await _repoEval.FindAll().ToListAsync();
            var list_ac = await _repoAc.FindAll(x => x.FunctionalLeader == userID && x.L0.Value).ToListAsync();
            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID
                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null 
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false
                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              UserID = y.Id,
                              FLID = y.FunctionalLeader.HasValue ? y.FunctionalLeader.Value : 0,
                              L0ID = y.Id,
                              L1ID = y.Manager.HasValue ? y.Manager.Value : 0,
                              L2ID = y.L2.HasValue ? y.L2.Value : 0,
                              isDisplayAttitudeBtn = display,
                              Type = "FL",
                              Dept = dept,

                              Center = center
                          }).Where(x => x.isDisplayAttitudeBtn).ToList();
            return result;
        }

        public async Task<List<EvaluationDto>> GetGMData(int userID)
        {
            bool isGm = _repoAc.FindById(userID).GM.Value;
            var list_ac = new List<Account>();
            if (isGm)
            {
                list_ac = await _repoAc.FindAll(x => x.GMScore.Value).ToListAsync();
            }
            var list_evaluation = await _repoEval.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              Name = y.FullName,
                              UserID = y.Id,
                              Type = "GM",
                              Dept = dept,
                              Center = center,
                          }).ToList();
            return result;
        }
    }
}
