using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using A4KPI.Constants;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;
using A4KPI.Helpers;
using System.Collections;

namespace A4KPI._Services.Services
{

    public class ReportService : IReportService
    {
        private readonly IAccountRepository _repoAc;
        private readonly IAccountGroupAccountRepository _repoAcA;
        private readonly IAccountTypeRepository _repoAcType;

        private readonly IKPINewRepository _repoKPINew;
        private readonly ITargetPICRepository _repoTargetPIC;
        private readonly IDoRepository _repoDo;
        private readonly IActionStatusRepository _repoAcs;
        private readonly IOCRepository _repoOC;
        private readonly IAccountRepository _repoAccount;
        private readonly ITypeRepository _repoType;
        private readonly ITargetRepository _repoTarget;
        private readonly ITargetYTDRepository _repoTargetYTD;
        private readonly IActionRepository _repoAction;
        private readonly IStatusRepository _repoStatus;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly ISettingMonthRepository _repoSettingMonthly;

        private readonly IEvaluationRepository _repoEval;
        private readonly ISystemFlowRepository _repoSystemFlow;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly ICampaignRepository _repoCampaign;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IEvaluationService _repoEvalService;
        public ReportService(
            IOCRepository repoOC,
            IEvaluationService repoEvalService,
            IUserRoleRepository repoUserRole,
            IRoleRepository repoRole,
            IEvaluationRepository repoEval,
            ISystemFlowRepository repoSystemFlow,
            IAttitudeSubmitRepository repoAttSubmit,
            ICampaignRepository repoCampaign,
            ITypeRepository repoType,
            IKPIAccountRepository repoKPIAc,
            IAccountGroupAccountRepository repoAcA,
            IAccountRepository repoAc,
            ITargetPICRepository repoTargetPIC,
            ITargetRepository repoTarget,
            ITargetYTDRepository repoTargetYTD,
            IActionStatusRepository repoAcs,
            IAccountTypeRepository repoAcType,
            IKPINewRepository repoKPINew,
            IDoRepository repoDo,
            IActionRepository repoAction,
            IStatusRepository repoStatus,
            ISettingMonthRepository repoSettingMonthly,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repoUserRole = repoUserRole;
            _repoEvalService = repoEvalService;
            _repoRole = repoRole;
            _repoOC = repoOC;
            _repoEval = repoEval;
            _repoSystemFlow = repoSystemFlow;
            _repoAttSubmit = repoAttSubmit;
            _repoCampaign = repoCampaign;
            _repoAc = repoAc;
            _repoAcType = repoAcType;
            _repoTargetPIC = repoTargetPIC;
            _repoAcs = repoAcs;
            _repoAcA = repoAcA;
            _repoKPIAc = repoKPIAc;
            _repoType = repoType;
            _repoTarget = repoTarget;
            _repoTargetYTD = repoTargetYTD;
            _repoKPINew = repoKPINew;
            _repoDo = repoDo;
            _repoAction = repoAction;
            _repoSettingMonthly = repoSettingMonthly;
            _repoStatus = repoStatus;
            _mapper = mapper;
            _configMapper = configMapper;
        }


        public async Task<object> TrackingAppaisalProgress(int userID, int campaignID)
        {
            var role_id = _repoUserRole.FindAll(x => x.UserID == userID).FirstOrDefault().RoleID;
            var role_guard = _repoRole.FindById(role_id).Code;
            var result = new List<ReportTrackingAppaisalDto>();
           
            var list_ac = new List<Account>();
            if (role_guard == SystemRole.SYSTEMADMIN || role_guard == SystemRole.GHRROLE)
            {
                list_ac = await _repoAc.FindAll(x => x.L0.Value).ToListAsync();
                foreach (var item_ac in list_ac)
                {
                    var result_tamp = new List<EvaluationDto>();
                    var funlead = item_ac.FunctionalLeader != null ? item_ac.FunctionalLeader.Value : 0;
                    var l0 = item_ac.Id;
                    var l1 = item_ac.Manager != null ? item_ac.Manager.Value : 0;
                    var l2 = item_ac.L2 != null ? item_ac.L2.Value : 0;

                    var flAppraisal = await GetFLFeedback(funlead, l0, role_guard,campaignID);
                    var selfAppraisal = await GetSelfAppraisal(l0,l0, role_guard, campaignID);
                    var firstAppraisal = await GetFirstLevelAppraisal(l1,l0, role_guard, campaignID);
                    var secondAppraisal = await GetSecondLevelAppraisal(l2,l0, role_guard, campaignID);

                    result_tamp.AddRange(flAppraisal);
                    result_tamp.AddRange(selfAppraisal);
                    result_tamp.AddRange(firstAppraisal);
                    result_tamp.AddRange(secondAppraisal);

                    foreach (var item in result_tamp)
                    {
                        // if (item.isDisplayAttitudeBtn)
                        // {
                        //     var item_add = new ReportTrackingAppaisalDto();
                        //     item_add.PIC = item.PIC;
                        //     item_add.L0 = item.Name;
                        //     item_add.Campaign = item.CampaignName;
                        //     item_add.Detail = SystemTrackingAppraisal.ATTITUDE;
                        //     result.Add(item_add);
                        // }

                        if (item.isDisplayKPIBtn)
                        {
                            var item_add = new ReportTrackingAppaisalDto();
                            item_add.PIC = item.PIC;
                            item_add.L0 = item.Name;
                            item_add.Campaign = item.CampaignName;
                            item_add.Detail = SystemTrackingAppraisal.KPI;
                            result.Add(item_add);
                        }

                        if (item.isDisplayNewAttBtn)
                        {
                            var item_add = new ReportTrackingAppaisalDto();
                            item_add.PIC = item.PIC;
                            item_add.L0 = item.Name;
                            item_add.Campaign = item.CampaignName;
                            item_add.Detail = SystemTrackingAppraisal.ATTITUDE;
                            result.Add(item_add);
                        }
                    }
                }
            } 
            else
            {
                var result_tamp = new List<EvaluationDto>();
                var flAppraisal = await GetFLFeedback(userID, 0,role_guard, campaignID);
                var selfAppraisal = await GetSelfAppraisal(userID,0, role_guard, campaignID);
                var firstAppraisal = await GetFirstLevelAppraisal(userID,0, role_guard, campaignID);
                var secondAppraisal = await GetSecondLevelAppraisal(userID,0, role_guard, campaignID);

                result_tamp.AddRange(flAppraisal);
                result_tamp.AddRange(selfAppraisal);
                result_tamp.AddRange(firstAppraisal);
                result_tamp.AddRange(secondAppraisal);

                foreach (var item in result_tamp)
                {
                    // if (item.isDisplayAttitudeBtn)
                    // {
                    //     var item_add = new ReportTrackingAppaisalDto();
                    //     item_add.PIC = item.PIC;
                    //     item_add.L0 = item.Name;
                    //     item_add.Campaign = item.CampaignName;
                    //     item_add.Detail = SystemTrackingAppraisal.ATTITUDE;

                    //     result.Add(item_add);

                    // }

                    if (item.isDisplayKPIBtn)
                    {
                        var item_add = new ReportTrackingAppaisalDto();
                        item_add.PIC = item.PIC;
                        item_add.L0 = item.Name;
                        item_add.Campaign = item.CampaignName;
                        item_add.Detail = SystemTrackingAppraisal.KPI;
                        result.Add(item_add);
                    }

                    if (item.isDisplayNewAttBtn)
                    {
                        var item_add = new ReportTrackingAppaisalDto();
                        item_add.PIC = item.PIC;
                        item_add.L0 = item.Name;
                        item_add.Campaign = item.CampaignName;
                        item_add.Detail = SystemTrackingAppraisal.ATTITUDE;

                        result.Add(item_add);

                    }

                }
            }

            var result_query = result.Where(x => x.PIC != SystemTrackingAppraisal.NA).OrderBy(x => x.PIC).ToList();
            int index = 1;
            result_query.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return result_query;
        }

        public async Task<List<EvaluationDto>> GetSelfAppraisal(int userID,int L0, string role_guard, int campaignID)
        {
            var list_ac = new List<Account>();
            var list_evaluation = new List<Evaluation>();
            if (role_guard == SystemRole.SYSTEMADMIN || role_guard == SystemRole.GHRROLE)
            {
                list_evaluation = await _repoEval.FindAll(x => x.UserID == L0 && x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.L0.Value && x.Id == L0).ToListAsync();
            }
            else
            {

                list_evaluation = await _repoEval.FindAll(x => x.UserID == userID && x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.L0.Value).ToListAsync();
            }
            

            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          let pic = _repoAc.FindById(y.Id) != null ? _repoAc.FindById(y.Id).FullName : "N/A"

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let displayKPIBtn = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL0KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              PIC = pic,
                              UserID = y.Id,
                              isDisplayAttitudeBtn = display,
                              isDisplayKPIBtn = displayKPIBtn,
                              isDisplayNewAttBtn = displayNewAtt,
                              Type = "L0",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude
                          }).Where(x => x.isDisplayNewAttBtn || x.isDisplayKPIBtn).ToList();
            return result;

        }

        public async Task<List<EvaluationDto>> GetFirstLevelAppraisal(int userID,int L0, string role_guard, int campaignID)
        {
            var list_ac = new List<Account>();
            var list_evaluation = new List<Evaluation>();

            if (role_guard == SystemRole.SYSTEMADMIN || role_guard == SystemRole.GHRROLE)
            {
                list_evaluation = await _repoEval.FindAll(x => x.UserID == L0 && x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.Manager == userID && x.Id == L0).ToListAsync();
            }
            else
            {

                list_evaluation = await _repoEval.FindAll(x => x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.Manager == userID && x.L0.Value && x.Manager > 0).ToListAsync();
            }
            

            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          let pic = _repoAc.FindById(y.Manager) != null ? _repoAc.FindById(y.Manager).FullName : "N/A"
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

                          //new-attitude
                          let displayNewAttFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false

                          let displayNewAttL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 1 || y.SystemFlow == 4 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false

                          // disable-enable button
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL1KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              PIC = pic,
                              UserID = y.Id,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtn = displayKPIBtn,

                              isDisplayNewAttBtnFL = displayNewAttFL,
                              isDisplayNewAttBtnL0 = displayNewAttL0,
                              isDisplayNewAttBtn = displayNewAtt,

                              Type = "L1",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude
                          }).Where(x
                          => x.isDisplayNewAttBtnL0 == false
                          && x.isDisplayNewAttBtnFL == false                          
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && (x.isDisplayNewAttBtn || x.isDisplayKPIBtn)).ToList();
            return result;

        }

        public async Task<List<EvaluationDto>> GetSecondLevelAppraisal(int userID,int L0, string role_guard, int campaignID)
        {
            
            var list_ac = new List<Account>();
            var list_evaluation = new List<Evaluation>();

            if (role_guard == SystemRole.SYSTEMADMIN || role_guard == SystemRole.GHRROLE)
            {
                list_evaluation = await _repoEval.FindAll(x => x.UserID == L0 && x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.L2 == userID && x.Id == L0).ToListAsync();
            }
            else
            {

                list_evaluation = await _repoEval.FindAll(x => x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.L2 == userID && x.L0.Value && x.L2 > 0).ToListAsync();
            }
            //list_evaluation = await _repoEval.FindAll().ToListAsync();
            //list_ac = await _repoAc.FindAll(x => x.L2 == userID && x.L0.Value && x.L2 > 0).ToListAsync();

            var list_systemFlow = await _repoSystemFlow.FindAll().ToListAsync();
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id
                          join z in list_systemFlow on y.SystemFlow equals z.SystemFlowID
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          let pic = _repoAc.FindById(y.L2) != null ? _repoAc.FindById(y.L2).FullName : "N/A"
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

                          //new-atttitude

                          let displayNewAttFL = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false

                          let displayNewAttL0 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL0 : false

                          let displayNewAttL1 = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL1 : false

                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? y.SystemFlow == 3 || y.SystemFlow == 6 ? false : xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL2 : false


                          //disable button kpi - attitude
                          let BtnAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2 : false

                          let BtnKPI = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnL2KPI : false

                          let BtnNewAttitude = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttL2 : false

                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              PIC = pic,
                              UserID = y.Id,

                              isDisplayAttitudeBtnFL = displayFL,
                              isDisplayAttitudeBtnL0 = displayL0,
                              isDisplayAttitudeBtnL1 = displayL1,
                              isDisplayAttitudeBtn = display,

                              isDisplayKPIBtnFL = displayKPIBtnFL,
                              isDisplayKPIBtnL0 = displayKPIBtnL0,
                              isDisplayKPIBtnL1 = displayKPIBtnL1,
                              isDisplayKPIBtn = displayKPIBtn,

                              isDisplayNewAttBtnFL= displayNewAttFL,
                              isDisplayNewAttBtnL0 = displayNewAttL0,
                              isDisplayNewAttBtnL1 = displayNewAttL1,
                              isDisplayNewAttBtn = displayNewAtt,
                              
                              Type = "L2",
                              Dept = dept,
                              Center = center,
                              BtnAttitude = !BtnAttitude,
                              BtnKPI = !BtnKPI,
                              BtnNewAttitude = !BtnNewAttitude

                          }).Where(x
                          =>
                          x.isDisplayNewAttBtnFL == false 
                          && x.isDisplayNewAttBtnL0 == false
                          && x.isDisplayNewAttBtnL1== false
                          && x.isDisplayKPIBtnFL == false
                          && x.isDisplayKPIBtnL0 == false
                          && x.isDisplayKPIBtnL1 == false
                          && (x.isDisplayNewAttBtn || x.isDisplayKPIBtn)).ToList();
            return result;


        }

        public async Task<List<EvaluationDto>> GetFLFeedback(int userID,int L0, string role_guard, int campaignID)
        {
            var list_ac = new List<Account>();
            var list_evaluation = new List<Evaluation>();
            if (role_guard == SystemRole.SYSTEMADMIN)
            {
                list_evaluation = await _repoEval.FindAll(x => x.UserID == L0 && x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.FunctionalLeader == userID && x.Id == L0).ToListAsync();
            }
            else
            {
                list_evaluation = await _repoEval.FindAll(x => x.CampaignID == campaignID).ToListAsync();
                list_ac = await _repoAc.FindAll(x => x.FunctionalLeader == userID && x.L0.Value).ToListAsync();
            }
            var list_attSubmit = await _repoAttSubmit.FindAll().ToListAsync();
            var list_campaign = await _repoCampaign.FindAll().ToListAsync();
            var result = (from x in list_evaluation
                          join y in list_ac on x.UserID equals y.Id  
                          join t in list_attSubmit on x.CampaignID equals t.CampaignID into xt
                          join c in list_campaign on x.CampaignID equals c.ID

                          let display = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnFL : false
                          let displayNewAtt = xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault() != null 
                          ? xt.Where(o => o.SubmitTo == y.Id).FirstOrDefault().BtnNewAttFL : false
                          let center = _repoOC.FindById(y.CenterId) != null ? _repoOC.FindById(y.CenterId).Name : null
                          let dept = _repoOC.FindById(y.DeptId) != null ? _repoOC.FindById(y.DeptId).Name : null

                          let pic = _repoAc.FindById(y.FunctionalLeader) != null ? _repoAc.FindById(y.FunctionalLeader).FullName : "N/A"
                          select new EvaluationDto
                          {
                              ID = x.ID,
                              CampaignID = x.CampaignID,
                              CampaignName = c.Name,
                              Name = y.FullName,
                              PIC = pic,
                              UserID = y.Id,
                              isDisplayAttitudeBtn = display,
                              isDisplayNewAttBtn = displayNewAtt,
                              Type = "FL",
                              Dept = dept,

                              Center = center
                          }).Where(x => x.isDisplayNewAttBtn).ToList();
            return result;
        }
        
        public async Task<object> ReportPDCA(DateTime currentTime, int userId)
        {
            var user = _repoAc.FindById(userId);
            var user_kpiAc = new List<KPIAccount>();
            if (user.FactId > 0 && user.CenterId > 0 && user.DeptId == 0)
            {
                user_kpiAc = await _repoKPIAc.FindAll(x => x.CenterId == user.CenterId).ToListAsync();
            }
            if (user.FactId > 0 && user.CenterId == 0 && user.DeptId == 0 || user.FactId == null)
            {
                user_kpiAc = await _repoKPIAc.FindAll(x => x.CenterId > 0).ToListAsync();
            }
            if (user.FactId > 0 && user.CenterId > 0 && user.DeptId > 0)
            {
                return user_kpiAc;
            }
            try
            {

                var list_tracking = (from x in user_kpiAc
                                    join z in _repoKPIAc.FindAll() on new { x.KpiId, x.AccountId } equals
                                    new { z.KpiId, z.AccountId } into total
                                    let center = _repoOC.FindById(x.CenterId) != null ? _repoOC.FindById(x.CenterId).Name : null
                                    let dept = _repoOC.FindById(x.DeptId) != null ? _repoOC.FindById(x.DeptId).Name : null
                                    join y in _repoKPINew.FindAll() on x.KpiId equals y.Id
                           join ac in _repoAc.FindAll() on x.AccountId equals ac.Id
                           select new 
                           {
                               UserId = ac.Id,
                               Center = center,
                               Dept = dept,
                               Start = y.StartDisplayMeetingTime,
                               End = y.EndDisplayMeetingTime,
                               FullName = ac.FullName,
                               Status = y.IsDisplayTodo,
                               Year = y.Year != null ? y.Year : y.CreatedTime.Year.ToString(),
                               TodoPending = GetActionAndPDCAofUser(currentTime, ac.Id).Result,
                               TodoTotal = total.Where(x => (x.KPINew.Year == null ? 
                               x.KPINew.CreatedTime.Year.ToString() == currentTime.Year.ToString() 
                               : x.KPINew.Year == currentTime.Year.ToString()) && x.KPINew.IsDisplayTodo == false
                               && x.KPINew.StartDisplayMeetingTime.Value.Month <= currentTime.Month && x.KPINew.EndDisplayMeetingTime.Value.Month >= currentTime.Month).Count()
                
                           }).Where(x => x.Year == currentTime.Year.ToString() && x.Status == false).ToList();

                var data = list_tracking.GroupBy(x => x.UserId).Select(x => new 
                {
                    FullName = x.First().FullName,
                    Center = x.First().Center,
                    Dept = x.First().Dept,
                    TodoPending = x.First().TodoPending,
                    TodoTotal = x.Sum(x => x.TodoTotal),
                });

                var result = data.Select(x => new TrackingProcessDto { 
                    FullName = x.FullName,
                    Center = x.Center,
                    Dept = x.Dept,
                    TodoPending = x.TodoPending,
                    TodoTotal = x.TodoTotal,
                    Percentage = Math.Round((x.TodoTotal.ToDouble() - x.TodoPending.ToDouble()) / x.TodoTotal.ToDouble() * 100, 0) + '%'.ToString(),
                }).ToList();

                var todoTotal = result.Sum(x => x.TodoTotal).ToDouble();
                var todoPending = result.Sum(x => x.TodoPending).ToDouble();
                var todoPer = Math.Round((todoTotal - todoPending) / todoTotal * 100, 0);

                return new TrackingProcessDataDto
                { 
                    TotalTracking = result,
                    TodoPending = todoPending,
                    TodoTotal = todoTotal,
                    Percentage = todoPer
                };
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
            throw new NotImplementedException();
        }

        public async Task<object> ReportPDCA2(DateTime currentTime, int userId)
        {
            var user = _repoAc.FindById(userId);
            var user_kpiAc = new List<KPIAccount>();
            var list = new List<TrackingKPIDto>();
            if (user.FactId > 0 && user.CenterId > 0 && user.DeptId == 0)
            {
                user_kpiAc = await _repoKPIAc.FindAll(x => x.CenterId == user.CenterId).ToListAsync();
            }
            if (user.FactId > 0 && user.CenterId == 0 && user.DeptId == 0 || user.FactId == null)
            {
                user_kpiAc = await _repoKPIAc.FindAll(x => x.CenterId > 0).ToListAsync();
            }
            if (user.FactId > 0 && user.CenterId > 0 && user.DeptId > 0)
            {
                return user_kpiAc;
            }
            try
            {
                foreach (var item in user_kpiAc.DistinctBy(x => x.AccountId))
                {
                    var data = await GetKPIofUser(currentTime, item.AccountId);
                    list.AddRange(data);
                    // foreach (var items in data)
                    // {
                    //     list.Add(items);
                    // }
                }

                int index = 1;
                list.ForEach(item =>
                {
                    item.Index = index;
                    index++;
                });
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }


            throw new NotImplementedException();
        }

        public async Task<List<TrackingKPIDto>> GetKPIofUser(DateTime currentTime, int userId)
        {
            var ct = currentTime;
            int accountId = userId;
          
            var date = currentTime;
            var month = date.Month;
            List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
           
            var latestMonth = ct.Month - 1;
            var month2 = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var year = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            //Display update PDCA
            var PDCA = await _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => new TrackingKPIDto
            {
                Id = x.KpiId,
                Start = x.KPINew.StartDisplayMeetingTime,
                End = x.KPINew.EndDisplayMeetingTime,
                Topic = x.KPINew.Name,
                Level = x.KPINew.Level,
                PICName =  _repoAc.FindById(accountId).FullName,
                Type = "UpdatePDCA",
                Year = x.KPINew.Year == null ? x.KPINew.CreatedTime.Year.ToString() : x.KPINew.Year,
                Status = x.KPINew.IsDisplayTodo,
                CurrentTarget = _repoTargetPIC.FindAll().Any(y => y.targetId == x.KPINew.Targets.FirstOrDefault(a => a.TargetTime.Year == currentTime.Year
                && a.TargetTime.Month == currentTime.Month).Id && y.AccountId == accountId && y.IsSubmit) ? false : true
            }).Where(x => x.CurrentTarget && x.Level != Level.Level_1).ToListAsync();

            // end

            return PDCA.Where(x => x.Year == currentTime.Year.ToString() && x.Status == false  && x.Status == false && x.Start.Value.Month <= month && x.End.Value.Month >= month).ToList();
        }

        private async Task<int> GetActionAndPDCAofUser(DateTime currentTime, int userId)
        {
            var ct = currentTime;
            int accountId = userId;
            var date = currentTime;
            var month = date.Month;
            List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
            
            var latestMonth = ct.Month - 1;
            var month2 = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var year = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            //Display update PDCA
            var PDCA = await _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => new
            {
                Id = x.KpiId,
                Start = x.KPINew.StartDisplayMeetingTime,
                End = x.KPINew.EndDisplayMeetingTime,
                Topic = x.KPINew.Name,
                Level = x.KPINew.Level,
                TypeText = _repoType.FindById(x.KPINew.TypeId).Description,
                Type = "UpdatePDCA",
                Year = x.KPINew.Year == null ? x.KPINew.CreatedTime.Year.ToString() : x.KPINew.Year,
                Status = x.KPINew.IsDisplayTodo,
                CurrentTarget = _repoTargetPIC.FindAll().Any(y => y.targetId == x.KPINew.Targets.FirstOrDefault(a => a.TargetTime.Year == currentTime.Year
                && a.TargetTime.Month == currentTime.Month).Id && y.AccountId == accountId && y.IsSubmit) ? false : true
            }).Where(x => x.CurrentTarget && x.Level != Level.Level_1).ToListAsync();

            return PDCA.Where(x => x.Year == currentTime.Year.ToString() && x.Status == false  && x.Status == false && x.Start.Value.Month <= month && x.End.Value.Month >= month).Count();
        }

        
    }
}
