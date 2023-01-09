using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;
using System.Net;
using A4KPI._Services.Interface;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace A4KPI._Services.Services
{
    public class AttitudeScoreService : IAttitudeScoreService
    {
        private OperationResult operationResult;
        private readonly IAttitudeScoreRepository _repo;
        private readonly IEvaluationRepository _repoEval;
        private readonly IAccountRepository _repoAc;
        private readonly IBehaviorCheckRepository _repoBehaviorCheck;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IScoreRepository _repoScore;
        private readonly IOCRepository _repoOC;
        private readonly IAttitudeHeadingRepository _repoAttHeading;
        private readonly IAttitudeCategoryRepository _repoAttCategory;
        private readonly IAttitudeKeypointRepository _repoAttKeypoint;
        private readonly IAttitudeBehaviorRepository _repoAttBehavior;
        private readonly IAttitudeAttchmentRepository _repoAttchment;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IKPINewRepository _repoKPINew;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly ITargetRepository _repoTarget;
        private readonly ICampaignRepository _repoCampaign;
        private readonly ITypeRepository _repoType;
        private readonly IActionRepository _repoAction;
        private readonly IDoRepository _repoDo;
        private readonly IPointRepository _repoPoint;
        private readonly ISystemFlowRepository _repoSystemFlow;
        private readonly IUserSystemFlowRepository _repoUserSystemFlow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKPIScoreRepository _repoKPIScore;
        private readonly ISpecialContributionScoreRepository _repoSpecialContribution;

        public AttitudeScoreService(
            IWebHostEnvironment currentEnvironment,
            IUserSystemFlowRepository repoUserSystemFlow,
            ISpecialContributionScoreRepository repoSpecialContribution,
            IKPIScoreRepository repoKPIScore,
            ISystemFlowRepository repoSystemFlow,
            ITargetRepository repoTarget,
            IAttitudeSubmitRepository repoAttSubmit,
            IPointRepository repoPoint,
            IActionRepository repoAction,
            IDoRepository repoDo,
            ITypeRepository repoType,
            ICampaignRepository repoCampaign,
            IAttitudeScoreRepository repo,
            IBehaviorCheckRepository repoBehaviorCheck,
            IScoreRepository repoScore,
            IAttitudeHeadingRepository repoAttHeading,
            IAttitudeCategoryRepository repoAttCategory,
            IAttitudeKeypointRepository repoAttKeypoint,
            IAttitudeBehaviorRepository repoAttBehavior,
            IAttitudeAttchmentRepository repoAttchment,
            IAccountRepository repoAc,
            IEvaluationRepository repoEval,
            IUserRoleRepository repoUserRole,
            IOCRepository repoOC,
            IRoleRepository repoRole,
            IMapper mapper,
            IKPINewRepository repoKPINew,
            IKPIAccountRepository repoKPIAc,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoSpecialContribution = repoSpecialContribution;
            _repoSystemFlow = repoSystemFlow;
            _repoKPIScore = repoKPIScore;
            _repoUserSystemFlow = repoUserSystemFlow;
            _repoPoint = repoPoint;
            _repoScore = repoScore;
            _repoAttSubmit = repoAttSubmit;
            _repoAction = repoAction;
            _repoDo = repoDo;
            _repoType = repoType;
            _repoCampaign = repoCampaign;
            _currentEnvironment = currentEnvironment;
            _repoBehaviorCheck = repoBehaviorCheck;
            _repoAttHeading = repoAttHeading;
            _repoAttCategory = repoAttCategory;
            _repoAttKeypoint = repoAttKeypoint;
            _repoAttBehavior = repoAttBehavior;
            _repoTarget = repoTarget;
            _repoAttchment = repoAttchment;
            _repoAc = repoAc;
            _repoEval = repoEval;
            _repoOC = repoOC;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _mapper = mapper;
            _configMapper = configMapper;
            _repoKPINew = repoKPINew;
            _repoKPIAc = repoKPIAc;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> UpdateReviseStation(ReviseStationDto model)
        {
            //var item = _repoAttSubmit.FindAll(x => x.SubmitTo == model.UserID && x.CampaignID == model.CampaignID).FirstOrDefault();
            //if (item != null)
            //{
            //    switch (model.KPIStation)
            //    {
            //        case "L0" :
            //            item.BtnL0KPI = true;
            //            break;
            //        case "L1":
            //            break;
            //        case "L2":
            //            break;
            //        default:
            //            break;
            //    }
            //}
            throw new NotImplementedException();
        }
        private async Task<bool> UpdateIsSubmitKPI(string type, int campaignID, int userID)
        {
            var item_update = _repoKPIScore.FindAll(
                            x => x.ScoreType.ToUpper() == type.ToUpper()
                            && x.CampaignID == campaignID && x.ScoreTo == userID).FirstOrDefault();
            if (item_update != null)
            {
                item_update.IsSubmit = !item_update.IsSubmit;
            }
            try
            {
                _repoKPIScore.Update(item_update);
                await _repoKPIScore.SaveAll();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<object> UpdateAttitudeSubmit(int campaignId, int submitTo, string currentKPI, string currentAtt)
        {
            var item = _repoAttSubmit.FindAll().FirstOrDefault(x =>x.CampaignID == campaignId && x.SubmitTo == submitTo);
            if (currentAtt != "NA")
            {
                var item_att_update = _repoAttSubmit.FindAll(o => o.SubmitTo == submitTo && o.CampaignID == campaignId).FirstOrDefault();
                switch (currentAtt.ToUpper())
                {
                    case "L0":
                       
                        item.BtnNewAttL0 = true;
                        item.BtnNewAttFL = false;
                        item.BtnNewAttL1 = false;
                        item.BtnNewAttL2 = false;

                        //update allow edit
                        item_att_update.IsSubmitAttitudeFL = false;
                        item_att_update.IsSubmitAttitudeL0 = false;
                        item_att_update.IsSubmitAttitudeL1 = false;
                        item_att_update.IsSubmitAttitudeL2 = false;

                        _repoAttSubmit.Update(item_att_update);

                        break;
                    case "FL":
                        item.BtnNewAttL0 = false;
                        item.BtnNewAttFL = true;
                        item.BtnNewAttL1 = false;
                        item.BtnNewAttL2 = false;
                        break;
                    case "L1":
                        
                        item.BtnNewAttL0 = false;
                        item.BtnNewAttFL = false;
                        item.BtnNewAttL1 = true;
                        item.BtnNewAttL2 = false;

                        //update allow edit
                        item_att_update.IsSubmitAttitudeL1 = false;
                        item_att_update.IsSubmitAttitudeL2 = false;
                        _repoAttSubmit.Update(item_att_update);

                        break;
                    case "L2":
                        
                        item.BtnNewAttL0 = false;
                        item.BtnNewAttFL = false;
                        item.BtnNewAttL1 = false;
                        item.BtnNewAttL2 = true;

                        //update allow edit
                        item_att_update.IsSubmitAttitudeL2 = false;
                        _repoAttSubmit.Update(item_att_update);
                        break;
                    default:
                        break;
                }
            }

            if (currentKPI != "NA")
            {
                switch (currentKPI)
                {
                    case "L0":
                        //if (currentKPI != "N/A")
                        //{
                        //    await UpdateIsSubmitKPI(currentKPI, campaignId, submitTo);
                        //}
                        item.BtnL0KPI = true;
                        item.BtnFLKPI = false;
                        item.BtnL1KPI = false;
                        item.BtnL2KPI = false;
                        //update allow edit kpi score

                        var allow_update_l0 = _repoKPIScore.FindAll(o => o.ScoreTo == submitTo).ToList();
                        foreach (var item_allow_update in allow_update_l0)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoKPIScore.UpdateRange(allow_update_l0);


                        //update allow edit special score

                        var allow_special_l0 = _repoSpecialContribution.FindAll(o => o.ScoreTo == submitTo).ToList();
                        foreach (var item_allow_update in allow_special_l0)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoSpecialContribution.UpdateRange(allow_special_l0);

                        break;
                    case "FL":
                        item.BtnL0KPI = false;
                        item.BtnFLKPI = true;
                        item.BtnL1KPI = false;
                        item.BtnL2KPI = false;
                        break;
                    case "L1":
                        //if (currentKPI != "N/A")
                        //{
                        //    await UpdateIsSubmitKPI(currentKPI, campaignId, submitTo);
                        //}
                        item.BtnL0KPI = false;
                        item.BtnFLKPI = false;
                        item.BtnL1KPI = true;
                        item.BtnL2KPI = false;

                        //update allow edit

                        var allow_update_l1 = _repoKPIScore.FindAll(o => o.ScoreTo == submitTo && (o.ScoreType == "L1" || o.ScoreType == "L2")).ToList();
                        foreach (var item_allow_update in allow_update_l1)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoKPIScore.UpdateRange(allow_update_l1);

                        //update allow edit special score

                        var allow_special_l1 = _repoSpecialContribution.FindAll(o => o.ScoreTo == submitTo && (o.ScoreType == "L1" || o.ScoreType == "L2")).ToList();
                        foreach (var item_allow_update in allow_special_l1)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoSpecialContribution.UpdateRange(allow_special_l1);
                        break;
                    case "L2":
                        //if (currentKPI != "N/A")
                        //{
                        //    await UpdateIsSubmitKPI(currentKPI, campaignId, submitTo);
                        //}
                        item.BtnL0KPI = false;
                        item.BtnFLKPI = false;
                        item.BtnL1KPI = false;
                        item.BtnL2KPI = true;

                        //update allow edit

                        var allow_update_l2 = _repoKPIScore.FindAll(o => o.ScoreTo == submitTo && o.ScoreType == "L2").ToList();
                        foreach (var item_allow_update in allow_update_l2)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoKPIScore.UpdateRange(allow_update_l2);

                        //update allow edit special score

                        var allow_special_l2 = _repoSpecialContribution.FindAll(o => o.ScoreTo == submitTo && o.ScoreType == "L2").ToList();
                        foreach (var item_allow_update in allow_special_l2)
                        {
                            item_allow_update.IsSubmit = !item_allow_update.IsSubmit;
                        }
                        _repoSpecialContribution.UpdateRange(allow_special_l2);
                        break;
                    default:
                        break;
                }
            }
            
            _repoAttSubmit.Update(item);
            await _repo.SaveAll();
            
            return new {
                status = true,
                message = "Successfully"
            };
                 
        }

        public async Task<object> GetAllScoreStation()
        {
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var list_ac = await _repoAc.FindAll(x => x.L0.Value).ToListAsync();
            var list_user_sysFlow = await _repoUserSystemFlow.FindAll().ToListAsync();
            var result = (from x in list_ac
                         from z in list_campaign
                         join y in list_user_sysFlow on x.SystemFlow equals y.SystemFlowID into list
                         select new
                         {
                            x.Id,
                            x.Username,
                            x.FullName,
                            x.SystemFlow,
                            z.Name,
                            CampaignID = z.ID,
                            KPIStation = list.Where(z => z.IsBtnKPI),
                            AttStation = list.Where(z => z.IsBtnAtt),
                            CurrentKPI = getCurrentKPI(x.Id , z.ID),
                            CurrentAtt = getCurrentAttitude(x.Id , z.ID)
                         }).ToList();
            return result;
        }

        private string getCurrentKPI(int userID, int campaignID)
        {
            var currentKPI = "N/A";
            var item_attSubmit = _repoAttSubmit.FindAll(x => x.SubmitTo == userID && x.CampaignID == campaignID).FirstOrDefault();
           
            if (item_attSubmit != null)
            {
                if (item_attSubmit.BtnL0KPI)
                {
                    currentKPI = ScoreType.L0;
                }
                if (item_attSubmit.BtnL1KPI)
                {
                    currentKPI = ScoreType.L1;
                }
                if (item_attSubmit.BtnL2KPI)
                {
                    currentKPI = ScoreType.L2;
                }
            }

            return currentKPI;
        }

        private string getCurrentAttitude(int userID, int campaignID)
        {
            var currentAtt = "N/A";
            var item_attSubmit = _repoAttSubmit.FindAll(x => x.SubmitTo == userID && x.CampaignID == campaignID).FirstOrDefault();
            if (item_attSubmit != null)
            {
                if (item_attSubmit.BtnNewAttFL)
                {
                    currentAtt = ScoreType.FL;
                }
                if (item_attSubmit.BtnNewAttL0)
                {
                    currentAtt = ScoreType.L0;
                }
                if (item_attSubmit.BtnNewAttL1)
                {
                    currentAtt = ScoreType.L1;
                }
                if (item_attSubmit.BtnNewAttL2)
                {
                    currentAtt = ScoreType.L2;
                }
            }

            return currentAtt;
        }

        public async Task<OperationResult> SubmitAttitude(SaveScoreDto model)
        {
            var att_submit = _repoAttSubmit.FindAll(x => x.SubmitTo == model.ScoreTo && x.CampaignID == model.CampaignID).FirstOrDefault();
            var systemFlow_user = _repoAc.FindAll(x => x.Id == model.ScoreTo).FirstOrDefault();
            switch (model.Type.ToUpper())
            {
                case "L0":
                    if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                    {
                        att_submit.L0Attitude = true;
                        att_submit.BtnL0 = false;
                        att_submit.BtnL2 = true;
                    }
                    else if(systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        att_submit.L0Attitude = true;
                        att_submit.BtnL0 = false;
                        att_submit.BtnL1 = true;
                    }
                    else
                    {
                        att_submit.L0Attitude = true;
                        att_submit.BtnL0 = false;
                        att_submit.BtnL1 = true;
                    }
                    break;
                case "L1":

                    if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    {
                        att_submit.L1Attitude = true;
                        att_submit.BtnL1 = false;
                        att_submit.BtnL2 = true;
                    }
                    else if (systemFlow_user.SystemFlow == 3 || systemFlow_user.SystemFlow == 6)
                    {
                        att_submit.L1Attitude = true;
                        att_submit.BtnL1 = false;
                        att_submit.BtnL2 = true;
                    }

                    break;
                case "L2":
                    //if (systemFlow_user.SystemFlow == 2 || systemFlow_user.SystemFlow == 5)
                    //{
                    //    att_submit.L2Attitude = true;
                    //    att_submit.BtnL1 = false;
                    //    att_submit.BtnL2 = true;
                    //}
                    //else if (systemFlow_user.SystemFlow == 1 || systemFlow_user.SystemFlow == 4)
                    //{
                    //    att_submit.L1Attitude = true;
                    //    att_submit.BtnL1 = false;
                    //    att_submit.BtnL2 = true;
                    //}

                    att_submit.L2Attitude = true;
                    att_submit.BtnL2 = false;
                    //att_submit.BtnL2KPI = false;
                    break;
                case "FL":
                    att_submit.FLAttitude = true;
                    att_submit.BtnFL = false;
                    att_submit.BtnFLKPI = false;
                    att_submit.BtnL0 = true;
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

        public async Task<object> GetPoint(string from, string to)
        {
            var result = await _repoPoint.FindAll(x => Convert.ToDouble(x.Value) >= Convert.ToDouble(from) && Convert.ToDouble(x.Value) <= Convert.ToDouble(to))
                .OrderByDescending(x => x.Value)
                .ToListAsync();
            return result;
        }

        public async Task<object> GetKPISelfScoreDefault(int campaignID, int userID, string type)
        {
            var current_year = DateTime.Now.Year;
            var list_kpiAc = await _repoKPIAc.FindAll(x => x.AccountId == userID).ToListAsync();
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();
            var result = (from x in list_kpiAc
                         join y in list_kpi on x.KpiId equals y.Id
                         join z in list_type on y.TypeId equals z.Id
                         select new
                         {
                             y.Id,
                             y.TypeId,
                             y.Name,
                             y.Year,
                             labels = GetListLabel(campaignID),
                             perfomances = GetListPerf(y.Id, campaignID),
                             targets = GetListTarget(y.Id, campaignID),
                             kpiType = z.Description
                         }).Where(x => x.kpiType != "string" && x.Year == current_year.ToString());
            return result;
        }

        public async Task<object> GetKPISelfScoreString(int campaignID, int userID, string type)
        {
            var current_year = DateTime.Now.Year;
            var list_kpiAc = await _repoKPIAc.FindAll(x => x.AccountId == userID).ToListAsync();
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();
            var result = (from x in list_kpiAc
                          join y in list_kpi on x.KpiId equals y.Id
                          join z in list_type on y.TypeId equals z.Id
                          select new
                          {
                              y.Id,
                              y.Name,
                              y.Year,
                              labels = GetListLabel(campaignID),
                              kpiType = z.Description,
                              data = GetListDataString(y.Id, campaignID , userID)
                          }).Where(x => x.kpiType == "string" && x.Year == current_year.ToString());
           
            return result;
        }


        private object GetListDataString(int kpiID, int campaignID, int userID)
        {
            var campaign = _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;
            var list_action = _repoAction.FindAll(x => x.KPIId == kpiID && x.AccountId == userID).ToList();
            var result = list_action.Select(x => new
            {
                x.Id,
                x.Content,
                x.Target,
                item = GetListItem(x.Id , campaignID)
            });

            
            return result;
        }

        private  object GetListLabel(int campaignID)
        {
            List<string> listLabels = new List<string>();
            List<int> listLabel = new List<int>();

            var campaign =  _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;
            if(start == 7 && end == 12)
            {
                for (int i = start - 1; i <= end - 1; i++)
                {
                    listLabel.Add(i);
                }
            }else
            {
                for (int i = start; i <= end; i++)
                {
                    listLabel.Add(i);
                }
            }

            foreach (var a in listLabel)
            {
                switch (a)
                {
                    case 1:
                        listLabels.Add("Jan"); break;
                    case 2:
                        listLabels.Add("Feb"); break;
                    case 3:
                        listLabels.Add("Mar"); break;
                    case 4:
                        listLabels.Add("Apr"); break;
                    case 5:
                        listLabels.Add("May"); break;
                    case 6:
                        listLabels.Add("Jun"); break;
                    case 7:
                        listLabels.Add("Jul"); break;
                    case 8:
                        listLabels.Add("Aug"); break;
                    case 9:
                        listLabels.Add("Sep"); break;
                    case 10:
                        listLabels.Add("Oct"); break;
                    case 11:
                        listLabels.Add("Nov"); break;
                    case 12:
                        listLabels.Add("Dec"); break;
                }
            }
            return listLabels;
        }

        private object GetListItem(int actionID, int campaignID)
        {
            List<string> listItems = new List<string>();
            List<int> listLabel = new List<int>();
            var campaign = _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;

            //for (int i = start; i <= end; i++)
            //{
            //    listLabel.Add(i);
            //}
            if (start == 7 && end == 12)
            {
                for (int i = start - 1; i <= end - 1; i++)
                {
                    listLabel.Add(i);
                }
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    listLabel.Add(i);
                }
            }
            foreach (var item in listLabel)
            {
                var result = _repoDo.FindAll(x => x.CreatedTime.Month == item && x.ActionId == actionID).FirstOrDefault();
                if (result != null)
                {
                    listItems.Add(result.Achievement);
                } else
                {
                    listItems.Add("-");
                }
            }
            return listItems;
        }

        private object GetListPerf(int kpiID, int campaignID)
        {
            List<double> listPerfomance = new List<double>();
            List<int> listLabel = new List<int>();
            var campaign = _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;
            var data = _repoTarget.FindAll(x => x.KPIId == kpiID).ToList();
            if (start == 7 && end == 12)
            {
                for (int i = start - 1; i <= end - 1; i++)
                {
                    listLabel.Add(i);
                }
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    listLabel.Add(i);
                }
            }
            //for (int i = start; i <= end; i++)
            //{
            //    listLabel.Add(i);
            //}
            foreach (var item in listLabel)
            {
                var yearNum = campaign.Year.ToInt();
                var dataExist = data.Where(x => x.TargetTime.Month == item && x.TargetTime.Year == yearNum).ToList();
                if (dataExist.Count > 0)
                {
                    var dataPerfomance = data.FirstOrDefault(x => x.TargetTime.Month == item && x.TargetTime.Year == yearNum).Performance;
                    listPerfomance.Add(dataPerfomance);
                }
                else
                {
                    listPerfomance.Add(0);
                }
            }
            return listPerfomance;
        }

        private object GetListTarget(int kpiID, int campaignID)
        {
            List<double> listTarget = new List<double>();
            List<int> listLabel = new List<int>();
            var campaign = _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;
            var data = _repoTarget.FindAll(x => x.KPIId == kpiID).ToList();

            if (start == 7 && end == 12)
            {
                for (int i = start - 1; i <= end - 1; i++)
                {
                    listLabel.Add(i);
                }
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    listLabel.Add(i);
                }
            }
            //for (int i = start; i <= end; i++)
            //{
            //    listLabel.Add(i);
            //}
            foreach (var item in listLabel)
            {
                var yearNum = campaign.Year.ToInt();
                var dataExist = data.Where(x => x.TargetTime.Month == item && x.TargetTime.Year == yearNum).ToList();
                if (dataExist.Count > 0)
                {
                    double dataTarget = data.FirstOrDefault(x => x.TargetTime.Month == item && x.TargetTime.Year == yearNum).Value;
                    listTarget.Add(dataTarget);
                }
                else
                {
                    listTarget.Add(0);
                }
            }
            return listTarget;
        }

        public async Task<List<AttitudeScoreDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<AttitudeScoreDto>(_configMapper).ToListAsync();
        }

        public async Task<List<AttitudeScoreDto>> GetAllByCampaignAsync(int campaignId)
        {
            var query = await _repo.FindAll(x => x.CampaignID == campaignId).ToListAsync();
            var result = (from a in query
                        select new AttitudeScoreDto
                        {
                                ID = a.ID,
                                AttitudeHeadingID = a.AttitudeHeadingID,
                                CampaignID = a.CampaignID,
                                AttitudeHeadingName = _repoAttHeading.FindById(a.AttitudeHeadingID).Name,
                                Comment = a.Comment,
                                Score = a.Score,
                                ScoreBy = a.ScoreBy,
                                ScoreTime = a.ScoreTime
                                
                        }).OrderBy(x => x.ID).ToList();
                return result;
           
        }

        public async Task<object> AddAsync(AttitudeScoreDto model)
        {
            var query = _repo.FindAll().FirstOrDefault(x => x.CampaignID == model.CampaignID && x.AttitudeHeadingID == model.AttitudeHeadingID);
            
            if (query == null && model.AttitudeHeadingID > 0)
            {
                var add = _mapper.Map<AttitudeScore>(model);
                _repo.Add(add);
                await _repo.SaveAll();
                
                return new {
                    status = true,
                    message = "Successfully"
                };
            }
            else
            {
                return new {
                    status = false,
                    message = "Attitude heading name already exists"
                };
            }
        }

        public async Task<object> UpdateAsync(AttitudeScoreDto model)
        {
            var query = _repo.FindAll().FirstOrDefault(x =>x.CampaignID == model.CampaignID && x.ID != model.ID && x.AttitudeHeadingID == model.AttitudeHeadingID);
            if (query == null && model.AttitudeHeadingID > 0)
            {
                var update = _mapper.Map<AttitudeScore>(model);
                _repo.Update(update);
                await _repo.SaveAll();
                
                return new {
                    status = true,
                    message = "Successfully"
                };
            }
            else
            {
                return new {
                    status = false,
                    message = "Attitude heading name already exists"
                };
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {

            var attitudeScore = _repo.FindById(id);
            
            _repo.Remove(attitudeScore);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,

                    Data = attitudeScore
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> GetAllAsync(int campaignID,int userFrom, int userTo, string type)
        {
            //string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //var user_system = JWTExtensions.GetDecodeTokenById(token).ToInt();

            var att = await _repo.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var list = new List<AttitudeScoreDataDto>();
            var list_result = new List<DataDto>();

            var list_PASSION = new AttitudeScoreDataDto();
            var list_ACCOUNTABILITY = new AttitudeScoreDataDto();
            var list_ATTENTION_TO_DETAIL = new AttitudeScoreDataDto();
            var list_EFFECTIVE_COMUNICATION = new AttitudeScoreDataDto();
            var list_RESILIENCE = new AttitudeScoreDataDto();
            var list_CONTINUOUS_LEARNING = new AttitudeScoreDataDto();

            foreach (var item in att)
            {
                var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
                var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
                var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
                var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

                //var Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userID)
                //    .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userID)
                //    .FirstOrDefault().Point : null;

                var Comment = GetComment(item.AttitudeHeadingID, campaignID, userFrom, userTo, type);
                var Score = GetScore(item.AttitudeHeadingID, campaignID, userFrom, userTo, type);

                var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                    .Select(x => new {
                        ID = x.ID,
                        Name = x.Name,
                        AttitudeHeadingID = x.AttitudeHeadingID
                    }).ToList();

                var heading_category = tamp.Select(x => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    CampaignID = campaignID,
                    AttitudeHeadingID = x.AttitudeHeadingID,
                    Score = Score,
                    Comment = Comment,
                    RowSpan = rowSpan,
                    Keypoint = GetKeypointData(x.ID, x.AttitudeHeadingID , userFrom, userTo, type)
                }).ToList();

                var items = new AttitudeScoreDataDto
                {
                    HeadingName = heading_name,
                    HeadingCode = heading_code,
                    HeadingID = item.AttitudeHeadingID,
                    RowSpan = rowSpan,
                    Definition = heading_defenition,
                    Score = Score,
                    Comment = Comment,
                    Data = heading_category,
                };

                switch (heading_code)
                {
                    case "PASSION":
                        list_PASSION = items;
                        break;
                    case "ACCOUNTABILITY":
                        list_ACCOUNTABILITY = items;
                        break;
                    case "ATTENTION_TO_DETAIL":
                        list_ATTENTION_TO_DETAIL = items;
                        break;
                    case "EFFECTIVE_COMUNICATION":
                        list_EFFECTIVE_COMUNICATION = items;
                        break;
                    case "RESILIENCE":
                        list_RESILIENCE = items;
                        break;
                    case "CONTINUOUS_LEARNING":
                        list_CONTINUOUS_LEARNING = items;
                        break;
                }
                //list.Add(items);

            }
            return new DataDto
            {
                Passion = list_PASSION,
                Accountbility = list_ACCOUNTABILITY,
                Attention = list_ATTENTION_TO_DETAIL,
                Effective = list_EFFECTIVE_COMUNICATION,
                Resilience = list_RESILIENCE,
                Continuous = list_CONTINUOUS_LEARNING
            };
        }

        


        private object GetKeypointData(int categoryID, int headingID, int userFrom, int userTo ,string type)
        {
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == headingID).ToList().Count;
            var list_keypoint_tamp = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == headingID && x.AttitudeCategoryID == categoryID)
                .Select(x => new { 
                    x.ID,
                    x.Name,
                    x.AttitudeCategoryID,
                    x.AttitudeHeadingID,
                    x.Level,
                    //Behavior = Behavior(x.ID)
                }).ToList();

            var list_keypoint = list_keypoint_tamp.Select(x => new
            {
                x.ID,
                x.Name,
                x.Level,
                x.AttitudeCategoryID,
                x.AttitudeHeadingID,
                Behavior = Behavior(x.ID, userFrom, userTo, type)
            }).OrderByDescending(x => x.Level).ToList();

            return list_keypoint;
        }

        private object GetKeypointDataDetail(int categoryID, int headingID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == headingID).ToList().Count;
            var list_keypoint_tamp = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == headingID && x.AttitudeCategoryID == categoryID)
                .Select(x => new {
                    x.ID,
                    x.Name,
                    x.AttitudeCategoryID,
                    x.AttitudeHeadingID,
                    x.Level,
                    //Behavior = Behavior(x.ID)
                }).ToList();

            var list_keypoint = list_keypoint_tamp.Select(x => new
            {
                x.ID,
                x.Name,
                x.Level,
                x.AttitudeCategoryID,
                x.AttitudeHeadingID,
                Behavior = BehaviorDetail(x.ID, flUser, l0User, l1User, l2User, type)
            }).OrderByDescending(x => x.Level).ToList();

            return list_keypoint;
        }

        private int GetRowSpan(int headingID)
        {
            var list_keypoint = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == headingID).ToList().Count;
            return list_keypoint;
        }

        private bool GetChecked(int BehaviorID, int userFrom, int userTo,string type)
        {
            bool check = false; 
            switch (type.ToUpper())
            {
                case "L0":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault().L0Checked : false ;
                    break;
                case "L1":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault().L1Checked : false;
                    break;
                case "L2":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault().L2Checked : false;
                    break;
                case "FL":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckFrom == userFrom && y.CheckTo == userTo).FirstOrDefault().FLChecked : false;
                    break;
                default:
                    break;
            }
            return check;
        }

        private bool GetCheckedDetail(int BehaviorID, int userFrom, int userTo, string type)
        {
            bool check = false;
            switch (type.ToUpper())
            {
                case "L0":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userTo).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userTo).FirstOrDefault().L0Checked : false;
                    break;
                case "L1":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault().L1Checked : false;
                    break;
                case "L2":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault().L2Checked : false;
                    break;
                case "FL":
                    check = _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault() != null
                        ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == BehaviorID && y.CheckBy == userFrom).FirstOrDefault().FLChecked : false;
                    break;
                default:
                    break;
            }
            return check;
        }

        private string GetComment(int AttitudeHeadingID, int campaignID , int userFrom, int userTo, string type)
        {
            string comment = string.Empty;
            switch (type.ToUpper())
            {
                case "L0":
                    comment = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L0Comment : null;
                    break;
                case "L1":
                    comment = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L1Comment : null;
                    break;
                case "L2":
                    comment = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L2Comment : null;
                    break;
                case "FL":
                    comment = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().FlComment : null;
                    break;
                default:
                    break;
            }
            return comment;
        }

        private string GetScore(int AttitudeHeadingID, int campaignID, int userFrom, int userTo, string type)
        {
            string score = string.Empty;
            switch (type.ToUpper())
            {
                case "L0":
                    score = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L0Score : null;
                    break;
                case "L1":
                    score = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L1Score : null;
                    break;
                case "L2":
                    score = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().L2Score : null;
                    break;
                case "FL":
                    score = _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo)
                        .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == userFrom && x.ScoreTo == userTo).FirstOrDefault().FLScore : null;
                    break;
                default:
                    break;
            }
            return score;
        }

        private object Behavior(int keypointID, int userFrom, int userTo, string type)
        {
            var list_tamp = _repoAttBehavior.FindAll(x => x.AttitudeKeypointID == keypointID)
                .Select(x => new { 
                    x.ID,
                    x.Name,
                    x.AttitudeKeypointID
                }).ToList();
            var list_behavior = list_tamp.Select(x => new
            {
                x.ID,
                x.Name,
                x.AttitudeKeypointID,
                HeadingID = _repoAttKeypoint.FindAll(y => y.ID == x.AttitudeKeypointID).FirstOrDefault() != null ? _repoAttKeypoint.FindAll(y => y.ID == x.AttitudeKeypointID).FirstOrDefault().AttitudeHeadingID : 0,
                Checked = GetChecked(x.ID , userFrom, userTo, type) 
            }).ToList();
            return list_behavior;
        }

        private object BehaviorDetail(int keypointID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var list_tamp = _repoAttBehavior.FindAll(x => x.AttitudeKeypointID == keypointID)
                .Select(x => new {
                    x.ID,
                    x.Name,
                    x.AttitudeKeypointID
                }).ToList();
            var list_behavior = list_tamp.Select(x => new
            {
                x.ID,
                x.Name,
                x.AttitudeKeypointID,
                HeadingID = _repoAttKeypoint.FindAll(y => y.ID == x.AttitudeKeypointID).FirstOrDefault() != null ? _repoAttKeypoint.FindAll(y => y.ID == x.AttitudeKeypointID).FirstOrDefault().AttitudeHeadingID : 0,

                L0 = _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l0User && y.CheckTo == l0User).FirstOrDefault() != null 
                ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l0User).FirstOrDefault().L0Checked 
                : false,

                L1 = _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l1User && y.CheckTo == l0User).FirstOrDefault() != null 
                ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l1User && y.CheckTo == l0User).FirstOrDefault().L1Checked 
                : false,

                L2 = _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l2User && y.CheckTo == l0User).FirstOrDefault() != null 
                ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == l2User && y.CheckTo == l0User).FirstOrDefault().L2Checked 
                : false,

                FL = _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == flUser && y.CheckTo == l0User).FirstOrDefault() != null 
                ? _repoBehaviorCheck.FindAll(y => y.BehaviorID == x.ID && y.CheckFrom == flUser && y.CheckTo == l0User).FirstOrDefault().FLChecked 
                : false,
                Height = ""
                //Checked = GetCheckedDetail(x.ID, userFrom, userTo, type)
            }).ToList();
            return list_behavior;
        }

        public async Task<OperationResult> SaveScore(SaveScoreDto model)
        {
            var att_score = _repoScore.FindAll(x => x.AttitudeHeadingID == model.HeadingID && x.CampaignID == model.CampaignID && x.ScoreBy == model.ScoreBy && x.ScoreTo == model.ScoreTo).FirstOrDefault();
            if (att_score == null)
            {

                var core_add = new Score();
                core_add.AttitudeHeadingID = model.HeadingID;
                core_add.CampaignID = model.CampaignID;
                core_add.ScoreTime = DateTime.Now;
                core_add.ScoreBy = model.ScoreBy;
                core_add.ScoreTo = model.ScoreTo;

                bool comment_required = false;
                double score_to = 0;
               
                switch (model.Type.ToUpper())
                {
                    case "L0":
                        //score_to = _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault() != null
                        //? _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault().FLScore.ToDouble() : 0;

                        core_add.L0Comment = model.Comment;
                        core_add.L0Score = model.Score;
                        
                        //if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                        //{
                        //    if(!string.IsNullOrEmpty(model.Comment)) {
                        //        comment_required = false;
                        //    }else {
                        //        comment_required = true;
                        //    }
                        //}
                        break;
                    case "L1":
                        score_to = _repoScore.FindAll(
                        x => x.AttitudeHeadingID == model.HeadingID
                        && x.CampaignID == model.CampaignID
                        && x.ScoreBy == model.ScoreTo
                        && x.ScoreTo == model.ScoreTo
                        ).FirstOrDefault() != null
                        ? _repoScore.FindAll(
                        x => x.AttitudeHeadingID == model.HeadingID
                        && x.CampaignID == model.CampaignID
                        && x.ScoreBy == model.ScoreTo
                        && x.ScoreTo == model.ScoreTo
                        ).FirstOrDefault().L0Score.ToDouble() : 0;

                        core_add.L1Comment = model.Comment;
                        core_add.L1Score = model.Score;
                        if (model.TypeBtn == "Next")
                        {
                            if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                            {
                                if(!string.IsNullOrEmpty(model.Comment)) {
                                    comment_required = false;
                                }else {
                                    comment_required = true;
                                }
                            }
                        }
                        break;
                    case "L2":
                        //score_to = _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreTo == model.ScoreTo).FirstOrDefault() != null
                        //? _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreTo == model.ScoreTo).FirstOrDefault().L1Score.ToDouble() : 0;

                        core_add.L2Comment = model.Comment;
                        core_add.L2Score = model.Score;
                        //if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                        //{
                        //    if(!string.IsNullOrEmpty(model.Comment)) {
                        //        comment_required = false;
                        //    }else {
                        //        comment_required = true;
                        //    }
                        //}
                        break;
                    case "FL":
                        core_add.FlComment = model.Comment;
                        core_add.FLScore = model.Score;
                        break;
                }
                if (comment_required)
                {
                    operationResult = new OperationResult
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "COMMENT_REQUIRED_MESSAGE",
                        Success = false
                    };

                    return operationResult;
                }
                _repoScore.Add(core_add);
            } else
            {
                att_score.ScoreTime = DateTime.Now;
                att_score.ScoreBy = model.ScoreBy;
                att_score.ScoreTo = model.ScoreTo;
                bool comment_required = false;
                double score_to = 0;
               
                switch (model.Type.ToUpper())
                {
                    case "L0":
                        //score_to = _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault() != null
                        //? _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault().FLScore.ToDouble() : 0;

                        att_score.L0Comment = model.Comment;
                        att_score.L0Score = model.Score;
                        //if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                        //{
                        //    if(!string.IsNullOrEmpty(model.Comment)) {
                        //        comment_required = false;
                        //    }else {
                        //        comment_required = true;
                        //    }
                        //}
                         
                        
                        break;
                    case "L1":
                        score_to = _repoScore.FindAll(
                        x => x.AttitudeHeadingID == model.HeadingID
                        && x.CampaignID == model.CampaignID
                        && x.ScoreBy == model.ScoreTo
                        && x.ScoreTo == model.ScoreTo
                        ).FirstOrDefault() != null
                        ? _repoScore.FindAll(
                        x => x.AttitudeHeadingID == model.HeadingID
                        && x.CampaignID == model.CampaignID
                        && x.ScoreBy == model.ScoreTo
                        && x.ScoreTo == model.ScoreTo
                        ).FirstOrDefault().L0Score.ToDouble() : 0;

                        att_score.L1Comment = model.Comment;
                        att_score.L1Score = model.Score;
                        if (model.TypeBtn == "Next")
                        {
                            if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                            {
                                if(!string.IsNullOrEmpty(model.Comment)) {
                                    comment_required = false;
                                }else {
                                    comment_required = true;
                                }
                            }
                        }

                        break;
                    case "L2":
                        //score_to = _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault() != null
                        //? _repoScore.FindAll(
                        //x => x.AttitudeHeadingID == model.HeadingID
                        //&& x.CampaignID == model.CampaignID
                        //&& x.ScoreBy == model.ScoreBy
                        //&& x.ScoreTo == model.ScoreTo
                        //).FirstOrDefault().L1Score.ToDouble() : 0;

                        att_score.L2Comment = model.Comment;
                        att_score.L2Score = model.Score;
                        //if (Math.Abs(score_to - model.Score.ToDouble()) >= 3 || Math.Abs(model.Score.ToDouble() - score_to) >= 3)
                        //{
                        //    if(!string.IsNullOrEmpty(model.Comment)) {
                        //        comment_required = false;
                        //    }else {
                        //        comment_required = true;
                        //    }
                        //}
                        break;
                    case "FL":
                        att_score.FlComment = model.Comment;
                        att_score.FLScore = model.Score;
                        //if (score_to - model.Score.ToDouble() >= 3 || model.Score.ToDouble() - score_to >= 3)
                        //{
                        //    comment_required = true;
                        //}
                        break;
                }
                if (comment_required)
                {
                    operationResult = new OperationResult
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "COMMENT_REQUIRED_MESSAGE",
                        Success = false
                    };

                    return operationResult;
                }
                _repoScore.Update(att_score);
            }
            
            var list_behaviorCheck_add = new List<BehaviorCheck>();
            var list_behaviorCheck_update = new List<BehaviorCheck>();
            foreach (var item in model.Data)
            {
                var check = _repoBehaviorCheck.FindAll(x => x.BehaviorID == item.BehaviorID && x.CampaignID == model.CampaignID && x.CheckFrom == model.ScoreBy && x.CheckTo == model.ScoreTo).FirstOrDefault();
                if (check == null)
                {
                    var item_add = new BehaviorCheck();
                    item_add.BehaviorID = item.BehaviorID;
                    item_add.CampaignID = model.CampaignID;
                    item_add.CheckBy = model.ScoreBy;
                    item_add.CheckFrom = model.ScoreBy;
                    item_add.CheckTo = model.ScoreTo;
                    switch (model.Type.ToUpper())
                    {
                        case "L0":
                            item_add.L0Checked = item.Checked;
                            break;
                        case "L1":
                            item_add.L1Checked = item.Checked;
                            break;
                        case "L2":
                            item_add.L2Checked = item.Checked;
                            break;
                        case "FL":
                            item_add.FLChecked = item.Checked;
                            break;

                        default:
                            break;
                    }
                    
                    item_add.CheckTime = DateTime.Now;
                    list_behaviorCheck_add.Add(item_add);
                } else
                {
                    check.CheckFrom = model.ScoreBy;
                    check.CheckTo = model.ScoreTo;
                    switch (model.Type.ToUpper())
                    {
                        case "L0":
                            check.L0Checked = item.Checked;
                            break;
                        case "L1":
                            check.L1Checked = item.Checked;
                            break;
                        case "L2":
                            check.L2Checked = item.Checked;
                            break;
                        case "FL":
                            check.FLChecked = item.Checked;
                            break;

                        default:
                            break;
                    }
                    list_behaviorCheck_update.Add(check);
                }
            }
            _repoBehaviorCheck.AddRange(list_behaviorCheck_add);
            _repoBehaviorCheck.UpdateRange(list_behaviorCheck_update);
            try
            {
                await _repoScore.SaveAll();
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

        public async Task<object> GetListCheckBehavior(int campaignID, int userFrom, int userTo, string type)
        {
            var list_behaviorCheck = await _repoBehaviorCheck.FindAll(x => x.CampaignID == campaignID && x.CheckFrom == userFrom && x.CheckTo == userTo).ToListAsync();
            var list_behavior = await _repoAttBehavior.FindAll().ToListAsync();
            var list_keypoint = await _repoAttKeypoint.FindAll().ToListAsync();
            var result_tamp = (from x in list_behaviorCheck
                               join y in list_behavior on x.BehaviorID equals y.ID
                               join z in list_keypoint on y.AttitudeKeypointID equals z.ID
                               select new
                               {
                                   x.ID,
                                   x.BehaviorID,
                                   x.CheckBy,
                                   x.CheckTime,
                                   x.CampaignID,
                                   z.AttitudeHeadingID
                               }).ToList();

            var result = result_tamp.Select(x => new
            {
                x.ID,
                x.BehaviorID,
                x.CheckBy,
                x.CheckTime,
                x.CampaignID,
                x.AttitudeHeadingID,
                Checked = GetChecked(x.BehaviorID, userFrom , userTo, type)
            }).ToList();
            return result;
        }


        public async Task<object> GetDetail(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var att = await _repo.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var list = new List<AttitudeScoreDataDto>();
            var list_result = new List<DataDto>();

            var list_PASSION = new AttitudeScoreDataDto();
            var list_ACCOUNTABILITY = new AttitudeScoreDataDto();
            var list_ATTENTION_TO_DETAIL = new AttitudeScoreDataDto();
            var list_EFFECTIVE_COMUNICATION = new AttitudeScoreDataDto();
            var list_RESILIENCE = new AttitudeScoreDataDto();
            var list_CONTINUOUS_LEARNING = new AttitudeScoreDataDto();

            foreach (var item in att)
            {
                var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
                var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
                var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
                var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

                var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                    .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                    .FirstOrDefault().L0Score : null;
                var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                    .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                    .FirstOrDefault().L1Score : null;
                var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                    .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                    .FirstOrDefault().L2Score : null;
                var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                    .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                    .FirstOrDefault().FLScore : null;

                //var Comment = GetComment(item.AttitudeHeadingID, campaignID, userFrom, type);

                var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                    _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

                var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                    _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

                var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                    _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

                var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                    _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

                var data = new List<AttitudeAttchment>();
                data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                       && x.HeadingID == item.AttitudeHeadingID
                       && x.UploadTo == l0User
                       ).ToListAsync();

                var files = data.Select(x => x.Path).ToList();
                var list_file = new List<DownloadFileDto>();
                files.ForEach(file =>
                {
                    string filePath = _currentEnvironment.WebRootPath + file;
                    var info = new FileInfo(filePath);
                    list_file.Add(new DownloadFileDto
                    {
                        Name = Path.GetFileName(filePath),
                        Path = file

                    });
                });
                var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                    .Select(x => new {
                        ID = x.ID,
                        Name = x.Name,
                        AttitudeHeadingID = x.AttitudeHeadingID
                    }).ToList();

                var heading_category = tamp.Select(x => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    CampaignID = campaignID,
                    AttitudeHeadingID = x.AttitudeHeadingID,
                    RowSpan = rowSpan,
                    Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
                }).ToList();

                var items = new AttitudeScoreDataDto
                {
                    HeadingName = heading_name,
                    HeadingCode = heading_code,
                    HeadingID = item.AttitudeHeadingID,
                    RowSpan = rowSpan,
                    L0Comment = L0_Comment,
                    L1Comment = L1_Comment,
                    L2Comment = L2_Comment,
                    FLComment = FL_Comment,
                    L0Score = L0_Score,
                    L1Score = L1_Score,
                    L2Score = L2_Score,
                    FLScore = FL_Score,
                    Definition = heading_defenition,
                    File = list_file,
                    Data = heading_category,
                };

                switch (heading_code)
                {
                    case "PASSION":
                        list_PASSION = items;
                        break;
                    case "ACCOUNTABILITY":
                        list_ACCOUNTABILITY = items;
                        break;
                    case "ATTENTION_TO_DETAIL":
                        list_ATTENTION_TO_DETAIL = items;
                        break;
                    case "EFFECTIVE_COMUNICATION":
                        list_EFFECTIVE_COMUNICATION = items;
                        break;
                    case "RESILIENCE":
                        list_RESILIENCE = items;
                        break;
                    case "CONTINUOUS_LEARNING":
                        list_CONTINUOUS_LEARNING = items;
                        break;
                }
                //list.Add(items);

            }
            return new DataDto
            {
                Passion = list_PASSION,
                Accountbility = list_ACCOUNTABILITY,
                Attention = list_ATTENTION_TO_DETAIL,
                Effective = list_EFFECTIVE_COMUNICATION,
                Resilience = list_RESILIENCE,
                Continuous = list_CONTINUOUS_LEARNING
            };
        }

        public async Task<object> GetDetailPassion(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.PASSION_NUMBER).FirstOrDefaultAsync();

            var list_PASSION = new AttitudeScoreDataDto();
            
            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_PASSION = items;
            return list_PASSION;
           
        }

        public async Task<object> GetDetailAccountbility(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.ACCOUNTABILITY_NUMBER).FirstOrDefaultAsync();
            var list_ACCOUNTABILITY = new AttitudeScoreDataDto();

            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_ACCOUNTABILITY = items;
            return list_ACCOUNTABILITY;
        }

        public async Task<object> GetDetailAttention(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.ATTENTION_TO_DETAIL_NUMBER).FirstOrDefaultAsync();
        
            var list_ATTENTION_TO_DETAIL = new AttitudeScoreDataDto();

            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_ATTENTION_TO_DETAIL = items;
            return list_ATTENTION_TO_DETAIL;
        }

        public async Task<object> GetDetailContinuous(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.CONTINUOUS_LEARNING_NUMBER).FirstOrDefaultAsync();
            var list_CONTINUOUS_LEARNING = new AttitudeScoreDataDto();

            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_CONTINUOUS_LEARNING = items;
            return list_CONTINUOUS_LEARNING;
        }

        public async Task<object> GetDetailEffective(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.EFFECTIVE_COMUNICATION_NUMBER).FirstOrDefaultAsync();
            var list_EFFECTIVE_COMUNICATION = new AttitudeScoreDataDto();
            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_EFFECTIVE_COMUNICATION = items;
            return list_EFFECTIVE_COMUNICATION;
        }

        public async Task<object> GetDetailResilience(int campaignID, int flUser, int l0User, int l1User, int l2User, string type)
        {
            var item = await _repo.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == SystemHeading.RESILIENCE_NUMBER).FirstOrDefaultAsync();
            var list_RESILIENCE = new AttitudeScoreDataDto();

            var heading_name = _repoAttHeading.FindById(item.AttitudeHeadingID).Name;
            var heading_defenition = _repoAttHeading.FindById(item.AttitudeHeadingID).Definition;
            var rowSpan = _repoAttKeypoint.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID).ToList().Count;
            var heading_code = _repoAttHeading.FindById(item.AttitudeHeadingID).Code;

            var L0_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User)
                .FirstOrDefault().L0Score : null;
            var L1_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User)
                .FirstOrDefault().L1Score : null;
            var L2_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User)
                .FirstOrDefault().L2Score : null;
            var FL_Score = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault() != null ? _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User)
                .FirstOrDefault().FLScore : null;


            var L0_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l0User && x.ScoreTo == l0User).FirstOrDefault().L0Comment : null;

            var L1_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l1User && x.ScoreTo == l0User).FirstOrDefault().L1Comment : null;

            var L2_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == l2User && x.ScoreTo == l0User).FirstOrDefault().L2Comment : null;

            var FL_Comment = _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault() != null ?
                _repoScore.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID && x.ScoreBy == flUser && x.ScoreTo == l0User).FirstOrDefault().FlComment : null;

            var data = new List<AttitudeAttchment>();
            data = await _repoAttchment.FindAll(x => x.CampaignID == campaignID
                    && x.HeadingID == item.AttitudeHeadingID
                    && x.UploadTo == l0User
                    ).ToListAsync();

            var files = data.Select(x => x.Path).ToList();
            var list_file = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list_file.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            var tamp = _repoAttCategory.FindAll(x => x.AttitudeHeadingID == item.AttitudeHeadingID && x.CampaignID == campaignID)
                .Select(x => new {
                    ID = x.ID,
                    Name = x.Name,
                    AttitudeHeadingID = x.AttitudeHeadingID
                }).ToList();

            var heading_category = tamp.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                CampaignID = campaignID,
                AttitudeHeadingID = x.AttitudeHeadingID,
                RowSpan = rowSpan,
                Keypoint = GetKeypointDataDetail(x.ID, x.AttitudeHeadingID, flUser, l0User, l1User, l2User, type)
            }).ToList();

            var items = new AttitudeScoreDataDto
            {
                HeadingName = heading_name,
                HeadingCode = heading_code,
                HeadingID = item.AttitudeHeadingID,
                RowSpan = rowSpan,
                L0Comment = L0_Comment,
                L1Comment = L1_Comment,
                L2Comment = L2_Comment,
                FLComment = FL_Comment,
                L0Score = L0_Score,
                L1Score = L1_Score,
                L2Score = L2_Score,
                FLScore = FL_Score,
                Definition = heading_defenition,
                File = list_file,
                Data = heading_category,
            };

            list_RESILIENCE = items;
            return list_RESILIENCE;
        }
    }
}
