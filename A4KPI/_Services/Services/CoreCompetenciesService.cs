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
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace A4KPI._Services.Services
{

    public class CoreCompetenciesService : ICoreCompetenciesService
    {
        private readonly IAccountRepository _repo;
        private readonly IOCRepository _repoOc;
        private readonly IKPINewRepository _repoKPINew;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IAccountRepository _repoAc;
        private readonly IEvaluationRepository _repoEvaluation;
        private readonly IKPIScoreRepository _repoKPIScore;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IKPIScoreAttchmentRepository _repoKPIScoreAttchment;
        private readonly IScoreRepository _repoScore;
        private readonly IAttitudeAttchmentRepository _repoAttitudeAttchment;
        private readonly ISpecialContributionScoreRepository _repoSpecialContributionScore;
        private readonly ICommitteeScoreRepository _repoCommitteeScore;
        private readonly IAttitudeHeadingRepository _repoAttitudeHeading;
        private readonly IHRCommentCmteeRepository _repoHRCommentCmtee;
        private readonly ICommitteeSequenceRepository _repoCommitteeSequence;
        private readonly ISystemLanguageRepository _repoSystemLanguage;
        private readonly IAttitudeSubmitRepository _repoAttitudeSubmit;
        private readonly IAttitudeCategoryRepository _repoAttCategory;
        private readonly IAttitudeKeypointRepository _repoAttKeypoint;
        private readonly IAttitudeBehaviorRepository _repoAttBehavior;
        private readonly IBehaviorCheckRepository _repoBehaviorCheck;
        private readonly INewAttitudeScoreRepository _repoNewAttitudeScore;
        private readonly INewAttitudeContentRepository _repoNewAttitudeContent;
        private readonly INewAttitudeAttchmentRepository _repoNewAttitudeAttchment;
        private readonly IAccountCampaignRepository _repoAccountCampaign;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;
        private readonly ITypeRepository _repoType;
        private readonly ICampaignRepository _repoCampaign;
        private readonly IDoRepository _repoDo;
        private readonly ITargetRepository _repoTarget;
        private readonly IActionRepository _repoAction;
        public CoreCompetenciesService(
            IAccountRepository repo,
            ICampaignRepository repoCampaign,
            IActionRepository repoAction,
            IDoRepository repoDo,
            ITargetRepository repoTarget,
            IOCRepository repoOC,
            ITypeRepository repoType,
            IKPINewRepository repoKPINew,
            IKPIAccountRepository repoKPIAc,
            IAccountRepository repoAc,
            IEvaluationRepository repoEvaluation,
            IKPIScoreRepository repoKPIScore,
            IKPIScoreAttchmentRepository repoKPIScoreAttchment,
            IScoreRepository repoScore,
            IAttitudeAttchmentRepository repoAttitudeAttchment,
            ISpecialContributionScoreRepository repoSpecialContributionScore,
            ICommitteeScoreRepository repoCommitteeScore,
            IAttitudeHeadingRepository repoAttitudeHeading,
            IHRCommentCmteeRepository repoHRCommentCmtee,
            ICommitteeSequenceRepository repoCommitteeSequence,
            ISystemLanguageRepository repoSystemLanguage,
            IAttitudeSubmitRepository repoAttitudeSubmit,
            IAttitudeCategoryRepository repoAttCategory,
            IAttitudeKeypointRepository repoAttKeypoint,
            IAttitudeBehaviorRepository repoAttBehavior,
            IBehaviorCheckRepository repoBehaviorCheck,
            INewAttitudeScoreRepository repoNewAttitudeScore,
            INewAttitudeContentRepository repoNewAttitudeContent,
            IAccountCampaignRepository repoAccountCampaign,
            INewAttitudeAttchmentRepository repoNewAttitudeAttchment,

            IWebHostEnvironment currentEnvironment,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoCampaign = repoCampaign;
            _repoDo = repoDo;
            _repoAction = repoAction;
            _repoTarget = repoTarget;
            _repoType = repoType;
            _repoOc = repoOC;
            _repoKPINew = repoKPINew;
            _repoKPIAc = repoKPIAc;
            _repoAc = repoAc;
            _repoEvaluation = repoEvaluation;
            _repoKPIScore = repoKPIScore;
            _repoKPIScoreAttchment = repoKPIScoreAttchment;
            _repoScore = repoScore;
            _repoAttitudeAttchment = repoAttitudeAttchment;
            _repoSpecialContributionScore = repoSpecialContributionScore;
            _repoCommitteeScore = repoCommitteeScore;
            _repoAttitudeHeading = repoAttitudeHeading;
            _repoHRCommentCmtee = repoHRCommentCmtee;
            _repoCommitteeSequence = repoCommitteeSequence;
            _repoSystemLanguage = repoSystemLanguage;
            _repoAttitudeSubmit = repoAttitudeSubmit;
            _repoAttCategory = repoAttCategory;
            _repoAttKeypoint = repoAttKeypoint;
            _repoAttBehavior = repoAttBehavior;
            _repoBehaviorCheck = repoBehaviorCheck;
            _repoNewAttitudeScore = repoNewAttitudeScore;
            _repoNewAttitudeContent = repoNewAttitudeContent;
            _repoAccountCampaign = repoAccountCampaign;
            _repoNewAttitudeAttchment = repoNewAttitudeAttchment;
            _currentEnvironment = currentEnvironment;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 


        public int GetPoint(NewAttitudeScore model)
        {
            int point = 0;
            point = model switch
                {
                    { Point1: true } => 1,
                    { Point2: true } => 2,
                    { Point3: true } => 3,
                    { Point4: true } => 4,
                    { Point5: true } => 5,
                    { Point6: true } => 6,
                    { Point7: true } => 7,
                    { Point8: true } => 8,
                    { Point9: true } => 9,
                    { Point10: true } => 10,
                    _ => 0
                };
            
            return point;
        }

        public async Task<List<CoreCompetenciesDto>> GetAllCoreCompetencies(string lang, int campaignID)
        {
            var index = 1;
            var queryAttitude = await _repoScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryL0 = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var result = (from a in queryAttitude
                        join b in queryL0 on a.ScoreTo equals b.Id
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLTW
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""
                        let l1 = _repo.FindById(b.Manager) != null ? _repo.FindById(b.Manager).FullName : ""
                        let l2 = _repo.FindById(b.L2) != null ? _repo.FindById(b.L2).FullName : ""
                        let fl = _repo.FindById(b.FunctionalLeader) != null ? _repo.FindById(b.FunctionalLeader).FullName : ""
                        let l0 = _repo.FindById(b.Id) != null ? _repo.FindById(b.Id).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreBy) != null ? _repo.FindById(a.ScoreBy).FullName : ""
                        let score = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Score : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Score : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Score : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FLScore : "0"
                        let comment = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Comment : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Comment : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Comment : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FlComment : ""
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? _repo.FindById(attitudeSubmit.SubmitTo) != null ? _repo.FindById(attitudeSubmit.SubmitTo).Id == a.ScoreBy ? attitudeSubmit.BtnL0 : _repo.FindById(attitudeSubmit.SubmitTo).Manager.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL1 : _repo.FindById(attitudeSubmit.SubmitTo).L2.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL2 : _repo.FindById(attitudeSubmit.SubmitTo).FunctionalLeader.ToInt() == a.ScoreBy ? attitudeSubmit.BtnFL : true : true : true
                        select new CoreCompetenciesDto()
                        {
                            Index = 0,
                            AttHeadingID = a.AttitudeHeadingID,
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            Score = score,
                            ScoreBy = scoreBy,
                            Comment = comment,
                            Submited = submited
                        }).Where(x => !x.Submited).OrderBy(x => x.L0).ThenBy(x => x.AttHeadingID).ToList();

        // Object[] newListData = result.Cast<object>().ToArray();

        // foreach (var item in newListData)
        // {
        //     item.index = index++;
        // }
            result.ForEach(x => {
                x.Index = index;
                index++;
            });

            return result;
        }

        public async Task<List<NewCoreCompetenciesDto>> GetAllNewCoreCompetencies(string lang, int campaignID)
        {
            var index = 1;
            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            
            var queryAc = await _repo.FindAll().ToListAsync();

            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : 
                        b.L1 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL1: 
                        b.L2 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL2 : 
                        b.FL == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeFL : false : false
                        select new NewAttitudeScorePointDto() {
                                        OrderNumber = a.OrderNumber,
                                        AttitudeContenID = a.AttitudeContenID,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        L1 = b.L1,
                                        L2 = b.L2,
                                        FL = b.FL,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();
            
            var result = (from a in list
                        join b in queryAc on a.ScoreTo equals b.Id
                        let content = _repoNewAttitudeContent.FindById(a.AttitudeContenID)
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLTW
                        let attBehavior = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == content.Behavior).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == content.Behavior).FirstOrDefault().SLTW
                        
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""

                        let l1 = _repo.FindById(a.L1) != null ? _repo.FindById(a.L1).FullName : ""
                        let l2 = _repo.FindById(a.L2) != null ? _repo.FindById(a.L2).FullName : ""
                        let fl = _repo.FindById(a.FL) != null ? _repo.FindById(a.FL).FullName : ""
                        
                        let l0 = _repo.FindById(a.ScoreTo) != null ? _repo.FindById(a.ScoreTo).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreFrom) != null ? _repo.FindById(a.ScoreFrom).FullName : ""

                        select new NewCoreCompetenciesDto()
                        {
                            Index = 0,
                            OrderNumber = a.OrderNumber.ToInt(),
                            AttHeading = attHeading,
                            AttBehavior = attBehavior,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            Score = a.Point,
                            ScoreBy = scoreBy
                        }).OrderBy(x => x.L0).ThenBy(x => x.OrderNumber).ThenBy(x => x.ScoreBy).ToList();
            
            result.ForEach(x => {
                x.Index = index;
                index++;
            });

            return result;
        }

        public async Task<List<NewCoreCompetenciesDto>> GetNewCoreCompetencies(string lang, int campaignID)
        {
            var index = 1;
            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            
            var queryAc = await _repo.FindAll().ToListAsync();
            
            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : 
                        b.L1 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL1: 
                        b.L2 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL2 : 
                        b.FL == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeFL : false : false
                        select new NewAttitudePointEquals2Dto() {
                                        OrderNumber = a.OrderNumber,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();

            var listAttitudeScore2 = list.ToList().GroupBy(x => new {   x.OrderNumber,
                                                                    x.ScoreTo,
                                                                    x.ScoreFrom,
                                                                })
                                    .Select(x => new NewAttitudePointEquals2Dto() {
                                        OrderNumber = x.First().OrderNumber,
                                        ScoreTo = x.First().ScoreTo,
                                        ScoreFrom = x.First().ScoreFrom,
                                        Point = Math.Round((x.Sum(y => y.Point.ToDouble()))/4, 2).ToString()
                                    }).ToList();


            var listAttitude = (from a in listAttitudeScore2
                        join c in accountCampaign on a.ScoreTo equals c.AccountID
                        join b in queryAc on a.ScoreTo equals b.Id
                        let content = _repoNewAttitudeContent.FindAll(x => x.OrderNumber == a.OrderNumber).FirstOrDefault()
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLTW
                        
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""

                        let l1 = _repo.FindById(c.L1) != null ? _repo.FindById(c.L1).FullName : ""
                        let l2 = _repo.FindById(c.L2) != null ? _repo.FindById(c.L2).FullName : ""
                        let fl = _repo.FindById(c.FL) != null ? _repo.FindById(c.FL).FullName : ""
                        
                        let l0 = _repo.FindById(c.AccountID) != null ? _repo.FindById(c.AccountID).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreFrom) != null ? _repo.FindById(a.ScoreFrom).FullName : ""

                        let comment = _repoNewAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreFrom && x.OrderNumber == a.OrderNumber).FirstOrDefault() != null ? 
                                      _repoNewAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreFrom && x.OrderNumber == a.OrderNumber).FirstOrDefault().Comment : ""
                        select new NewCoreCompetenciesDto()
                        {
                            Index = 0,
                            OrderNumber = a.OrderNumber.ToInt(),
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            Score = a.Point,
                            Comment = comment,
                            ScoreBy = scoreBy
                        }).OrderBy(x => x.L0).ThenBy(x => x.OrderNumber).ThenBy(x => x.ScoreBy).ToList();
            
            listAttitude.ForEach(x => {
                x.Index = index;
                index++;
            });

            return listAttitude;
        }

        public async Task<List<NewCoreCompetenciesDto>> GetNewCoreCompetenciesScoreEquals2(string lang, int campaignID)
        {
            var index = 1;
            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            
            var queryAc = await _repo.FindAll().ToListAsync();
            
            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : 
                        b.L1 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL1: 
                        b.L2 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL2 : 
                        b.FL == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeFL : false : false
                        select new NewAttitudePointEquals2Dto() {
                                        OrderNumber = a.OrderNumber,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();

            var listAttitudeScore2 = list.ToList().GroupBy(x => new {   x.OrderNumber,
                                                                    x.ScoreTo,
                                                                    x.ScoreFrom,
                                                                })
                                    .Select(x => new NewAttitudePointEquals2Dto() {
                                        OrderNumber = x.First().OrderNumber,
                                        ScoreTo = x.First().ScoreTo,
                                        ScoreFrom = x.First().ScoreFrom,
                                        Point = Math.Round((x.Sum(y => y.Point.ToDouble()))/4, 2).ToString()
                                    }).Where(x => x.Point.ToDouble() <= 2).ToList();


            var result = (from a in listAttitudeScore2
                        join c in accountCampaign on a.ScoreTo equals c.AccountID
                        join b in queryAc on a.ScoreTo equals b.Id
                        let content = _repoNewAttitudeContent.FindAll(x => x.OrderNumber == a.OrderNumber).FirstOrDefault()
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLTW
                        
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""

                        let l1 = _repo.FindById(c.L1) != null ? _repo.FindById(c.L1).FullName : ""
                        let l2 = _repo.FindById(c.L2) != null ? _repo.FindById(c.L2).FullName : ""
                        let fl = _repo.FindById(c.FL) != null ? _repo.FindById(c.FL).FullName : ""
                        
                        let l0 = _repo.FindById(c.AccountID) != null ? _repo.FindById(c.AccountID).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreFrom) != null ? _repo.FindById(a.ScoreFrom).FullName : ""

                        let comment = _repoNewAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreFrom && x.OrderNumber == a.OrderNumber).FirstOrDefault() != null ? 
                                      _repoNewAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreFrom && x.OrderNumber == a.OrderNumber).FirstOrDefault().Comment : ""
                        select new NewCoreCompetenciesDto()
                        {
                            Index = 0,
                            OrderNumber = a.OrderNumber.ToInt(),
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            Score = a.Point,
                            ScoreBy = scoreBy
                        }).OrderBy(x => x.L0).ThenBy(x => x.OrderNumber).ThenBy(x => x.ScoreBy).ToList();
                                    
            
            result.ForEach(x => {
                x.Index = index;
                index++;
            });

            return result;
        }

        public async Task<object> GetNewCoreCompetenciesScoreThan2(string lang, int campaignID)
        {
            var index = 1;
            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            
            var queryAc = await _repo.FindAll().ToListAsync();
            
            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : 
                        b.L1 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL1: 
                        b.L2 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL2 : 
                        b.FL == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeFL : false : false
                        select new NewAttitudePointEquals2Dto() {
                                        OrderNumber = a.OrderNumber,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();

            var listAttitudeScore = list.ToList().GroupBy(x => new {   x.OrderNumber,
                                                                    x.ScoreTo,
                                                                    x.ScoreFrom,
                                                                })
                                    .Select(x => new NewAttitudePointEquals2Dto() {
                                        OrderNumber = x.First().OrderNumber,
                                        ScoreTo = x.First().ScoreTo,
                                        ScoreFrom = x.First().ScoreFrom,
                                        Point = Math.Round((x.Sum(y => y.Point.ToDouble()))/4, 2).ToString()
                                    }).ToList();
            
            
            
            var result = (from a in listAttitudeScore
                        join c in accountCampaign on a.ScoreTo equals c.AccountID
                        join b in queryAc on a.ScoreTo equals b.Id
                        let content = _repoNewAttitudeContent.FindAll(x => x.OrderNumber == a.OrderNumber).FirstOrDefault()
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == content.Name).FirstOrDefault().SLTW
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""
                        
                        let l1 = _repo.FindById(c.L1) != null ? _repo.FindById(c.L1).FullName : ""
                        let l2 = _repo.FindById(c.L2) != null ? _repo.FindById(c.L2).FullName : ""
                        let fl = _repo.FindById(c.FL) != null ? _repo.FindById(c.FL).FullName : ""
                        
                        let l0 = _repo.FindById(a.ScoreTo) != null ? _repo.FindById(a.ScoreTo).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreFrom) != null ? _repo.FindById(a.ScoreFrom).FullName : ""

                        let score = a.Point
                        
                        let selfScore = listAttitudeScore.FirstOrDefault(x => x.OrderNumber == a.OrderNumber && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreTo) != null ? 
                        listAttitudeScore.FirstOrDefault(x => x.OrderNumber == a.OrderNumber && x.ScoreTo == a.ScoreTo && x.ScoreFrom == a.ScoreTo).Point : "0"
                        
                        select new NewCoreCompetenciesDto()
                        {
                            Index = 0,
                            OrderNumber = a.OrderNumber.ToInt(),
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            SelfScore = selfScore,
                            Score = score,
                            ScoreBy = scoreBy
                        }).ToList();

            var resultThan2 = result.Where(x => x.ScoreBy != x.L0 && (
                            (x.SelfScore.ToDouble() - x.Score.ToDouble() >= 2 && x.SelfScore.ToDouble() - x.Score.ToDouble() < 3) || 
                            (x.Score.ToDouble() - x.SelfScore.ToDouble() >= 2 && x.Score.ToDouble() - x.SelfScore.ToDouble() < 3)
                            ))
                        .OrderBy(x => x.L0).ThenBy(x => x.ScoreBy).ThenBy(x => x.OrderNumber).ToList();

            var resultThan3 = result.Where(x => x.ScoreBy != x.L0 && (
                            (x.SelfScore.ToDouble() - x.Score.ToDouble() >= 3) || 
                            (x.Score.ToDouble() - x.SelfScore.ToDouble() >= 3)
                            ))
                        .OrderBy(x => x.L0).ThenBy(x => x.ScoreBy).ThenBy(x => x.OrderNumber).ToList();

            resultThan2.ForEach(x => {
                x.Index = index;
                index++;
            });

            resultThan3.ForEach(x => {
                x.Index = index;
                index++;
            });
            
            return new { 
                            resultThan2,
                            resultThan3
                        };
        }

        public async Task<List<CoreCompetenciesAverageDto>> GetNewCoreCompetenciesAverage(string lang, int campaignID)
        {
            var index = 1;
            var queryAttHeading = _repoNewAttitudeContent.FindAll().DistinctBy(x => x.OrderNumber).ToList();

            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            
            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let type = a.ScoreFrom == a.ScoreTo ? ScoreType.L0 : 
                        a.ScoreFrom == b.L1 ? ScoreType.L1 : 
                        a.ScoreFrom == b.L2 ? ScoreType.L2 : ScoreType.FL
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : 
                        b.L1 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL1: 
                        b.L2 == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL2 : 
                        b.FL == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeFL : false : false
                        select new NewAttitudePointEquals2Dto() {
                                        OrderNumber = a.OrderNumber,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        Type = type,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();

            var listAttitudeScore = list.ToList().GroupBy(x => new {x.OrderNumber,
                                                                    x.ScoreTo,
                                                                    x.ScoreFrom,
                                                                    x.Type
                                                                })
                                    .Select(x => new NewAttitudePointEquals2Dto() {
                                        OrderNumber = x.First().OrderNumber,
                                        ScoreTo = x.First().ScoreTo,
                                        ScoreFrom = x.First().ScoreFrom,
                                        Type = x.First().Type,
                                        Point = Math.Round((x.Sum(y => y.Point.ToDouble()))/4, 2).ToString()
                                    }).ToList();
            
            var data =  (from a in queryAttHeading
                         let factory = _repoOc.FindById(_repoAc.FindById(accountCampaign.FirstOrDefault().AccountID).FactId).Name
                         let attHeadingName = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLTW

                         select new CoreCompetenciesAverageDto () {
                            Index = a.OrderNumber.ToString(),
                            Factory = factory,
                            AttHeading = attHeadingName,
                            L0Average = NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L0).ToList()).Item3,
                            L1Average = NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L1).ToList()).Item3,
                            L2Average = NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L2).ToList()).Item3,
                            FLAverage = NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.FL).ToList()).Item3,
                            FtyAverage = String.Format("{0:0.00}", Math.Round(((NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L0).ToList()).Item1 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L1).ToList()).Item1 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L2).ToList()).Item1 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.FL).ToList()).Item1)/
                                                                                (NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L0).ToList()).Item2 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L1).ToList()).Item2 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.L2).ToList()).Item2 + 
                                                                                NewAverage(a.OrderNumber.Value, listAttitudeScore.Where(x => x.Type == ScoreType.FL).ToList()).Item2)), 1))
                         }).ToList();

            data.ForEach(x => {
                x.Index = index.ToString();
                index++;
            });
            
            var item = new CoreCompetenciesAverageDto() {
                            Index = null,
                            Factory = data.FirstOrDefault().Factory,
                            AttHeading = lang == SystemLang.EN ? "Total score" : "總分",
                            L0Average = String.Format("{0:0.00}",data.Sum(x => x.L0Average.ToDouble())),
                            L1Average = String.Format("{0:0.00}",data.Sum(x => x.L1Average.ToDouble())),
                            L2Average = String.Format("{0:0.00}",data.Sum(x => x.L2Average.ToDouble())),
                            FLAverage = String.Format("{0:0.00}",data.Sum(x => x.FLAverage.ToDouble())),
                            FtyAverage = String.Format("{0:0.00}",data.Sum(x => x.FtyAverage.ToDouble()))
            };
            data.Add(item);

            return data;
        }

        private Tuple<double, double, string> NewAverage(int attHeading, List<NewAttitudePointEquals2Dto> data ) {
            var listScore = (from a in data.FindAll(x => x.OrderNumber == attHeading )
                                select new {
                                    Score = a.Point,
                                }).ToList();
            double total = listScore.Count();
            double totalScore = listScore.Sum(x => x.Score.ToDouble());
            double average = totalScore / total;
            // average = Math.Round(average, 2);
            return Tuple.Create(totalScore, total, String.Format("{0:0.00}", average)); 
            // return Tuple.Create(totalScore, total, average.ToString()); 
        }

        public async Task<List<CoreCompetenciesPercentileDto>> GetNewCoreCompetenciesPercentile(string lang, int campaignID)
        {
            var index = 1;
            var queryAttHeading = _repoNewAttitudeContent.FindAll().DistinctBy(x => x.OrderNumber).ToList();

            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryNewAttitude = await _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID && x.ScoreFrom == x.ScoreTo).ToListAsync();
            
            var list = (from a in queryNewAttitude
                        join b in accountCampaign on a.ScoreTo equals b.AccountID
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? 
                        b.AccountID == a.ScoreFrom ? attitudeSubmit.IsSubmitAttitudeL0 : false : false
                        select new NewAttitudePointEquals2Dto() {
                                        OrderNumber = a.OrderNumber,
                                        CampaignID = a.CampaignID,
                                        ScoreFrom = a.ScoreFrom,
                                        ScoreTo = a.ScoreTo,
                                        Point = GetPoint(a).ToString(),
                                        Submited = submited
                                    }).Where(x => x.Submited).ToList();

            var listAttitudeScore = list.ToList().GroupBy(x => new {x.OrderNumber,
                                                                    x.ScoreTo,
                                                                    x.ScoreFrom,
                                                                })
                                    .Select(x => new NewAttitudePointEquals2Dto() {
                                        OrderNumber = x.First().OrderNumber,
                                        ScoreTo = x.First().ScoreTo,
                                        ScoreFrom = x.First().ScoreFrom,
                                        Point = Math.Round((x.Sum(y => y.Point.ToDouble()))/4, 2).ToString()
                                    }).ToList();

            var data =  (from a in queryAttHeading
                         let factory = _repoOc.FindById(_repoAc.FindById(accountCampaign.FirstOrDefault().AccountID).FactId).Name
                         let attHeadingName = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLTW
                         let countL0 = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).Count().ToDouble()
                         let p10 = Math.Ceiling((countL0 * 10/100).ToDouble()) - 1
                         let p25 = Math.Ceiling((countL0 * 25/100).ToDouble()) - 1
                         let p50 = Math.Ceiling((countL0 * 50/100).ToDouble()) - 1
                         let p75 = Math.Ceiling((countL0 * 75/100).ToDouble()) - 1
                         let p90 = Math.Ceiling((countL0 * 90/100).ToDouble()) - 1

                         let p10Score = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).OrderBy(x => x.Point.ToDouble()).ThenBy(x => x.ScoreTo).ToList()[p10.ToInt()].Point
                         let p25Score = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).OrderBy(x => x.Point.ToDouble()).ThenBy(x => x.ScoreTo).ToList()[p25.ToInt()].Point
                         let p50Score = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).OrderBy(x => x.Point.ToDouble()).ThenBy(x => x.ScoreTo).ToList()[p50.ToInt()].Point
                         let p75Score = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).OrderBy(x => x.Point.ToDouble()).ThenBy(x => x.ScoreTo).ToList()[p75.ToInt()].Point
                         let p90Score = listAttitudeScore.Where(x => x.OrderNumber == a.OrderNumber).OrderBy(x => x.Point.ToDouble()).ThenBy(x => x.ScoreTo).ToList()[p90.ToInt()].Point
                         
                         select new CoreCompetenciesPercentileDto () {
                            Index = a.OrderNumber.ToString(),
                            Factory = factory,
                            AttHeading = attHeadingName,
                            P10 = String.Format("{0:0.00}", p10Score.ToDouble()),
                            P25 = String.Format("{0:0.00}", p25Score.ToDouble()),
                            P50 = String.Format("{0:0.00}", p50Score.ToDouble()),
                            P75 = String.Format("{0:0.00}", p75Score.ToDouble()),
                            P90 = String.Format("{0:0.00}", p90Score.ToDouble())
                         }).ToList();

            data.ForEach(x => {
                x.Index = index.ToString();
                index++;
            });
            
            var item = new CoreCompetenciesPercentileDto() {
                            Index = null,
                            Factory = data.FirstOrDefault().Factory,
                            AttHeading = lang == SystemLang.EN ? "Total score" : "總分",
                            P10 = String.Format("{0:0.00}",data.Sum(x => x.P10.ToDouble())),
                            P25 = String.Format("{0:0.00}",data.Sum(x => x.P25.ToDouble())),
                            P50 = String.Format("{0:0.00}",data.Sum(x => x.P50.ToDouble())),
                            P75 = String.Format("{0:0.00}",data.Sum(x => x.P75.ToDouble())),
                            P90 = String.Format("{0:0.00}",data.Sum(x => x.P90.ToDouble()))
            };
            data.Add(item);

            return data;
        }

        public async Task<byte[]> ExportExcelNewCoreCompetencies(string lang, int campaignID)
        {
            return await ExportExcelConsumptionCase2(lang, campaignID);
            //throw new NotImplementedException();
        }
        
        private async Task<Byte[]> ExportExcelConsumptionCase2(string lang, int campaignID)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Huu Quynh";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "CoreCompetencies";

                    var listAllCoreCompetencies = new List<NewCoreCompetenciesDto>();
                    listAllCoreCompetencies = await GetAllNewCoreCompetencies(lang, campaignID);

                    var listCoreCompetenciesScore = new List<NewCoreCompetenciesDto>();
                    listCoreCompetenciesScore = await GetNewCoreCompetencies(lang, campaignID);

                    var listCoreCompetenciesScoreEquals2 = new List<NewCoreCompetenciesDto>();
                    listCoreCompetenciesScoreEquals2 = await GetNewCoreCompetenciesScoreEquals2(lang, campaignID);

                    dynamic listScoreThan2andThan3 = await GetNewCoreCompetenciesScoreThan2(lang, campaignID);
                    var listCoreCompetenciesScoreThan2 = new List<NewCoreCompetenciesDto>();
                    var listCoreCompetenciesScoreThan3 = new List<NewCoreCompetenciesDto>();
                    
                    listCoreCompetenciesScoreThan2 = listScoreThan2andThan3.resultThan2;
                    listCoreCompetenciesScoreThan3 = listScoreThan2andThan3.resultThan3;
                    
                    var listCoreCompetenciesAverage = new List<CoreCompetenciesAverageDto>();
                    listCoreCompetenciesAverage = await GetNewCoreCompetenciesAverage(lang, campaignID);
                    
                    var listCoreCompetenciesPercentile = new List<CoreCompetenciesPercentileDto>();
                    listCoreCompetenciesPercentile = await GetNewCoreCompetenciesPercentile(lang, campaignID);
                    
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("AllCoreCompetencies");
                    p.Workbook.Worksheets.Add("CoreCompetencies");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreEquals2");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreThan2");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreThan3");
                    p.Workbook.Worksheets.Add("CoreCompetenciesAverage");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet wsAll = p.Workbook.Worksheets["AllCoreCompetencies"];
                    ExcelWorksheet ws = p.Workbook.Worksheets["CoreCompetencies"];
                    ExcelWorksheet wsScoreEquals2 = p.Workbook.Worksheets["CoreCompetenciesScoreEquals2"];
                    ExcelWorksheet wsScoreThan2 = p.Workbook.Worksheets["CoreCompetenciesScoreThan2"];
                    ExcelWorksheet wsScoreThan3 = p.Workbook.Worksheets["CoreCompetenciesScoreThan3"];
                    ExcelWorksheet wsAverage = p.Workbook.Worksheets["CoreCompetenciesAverage"];

                    // đặt tên cho sheet
                    wsAll.Name = "行為指標分數";
                    ws.Name = "核心職能資料分析";
                    wsScoreEquals2.Name = "核心職能評2分項目";
                    wsScoreThan2.Name = "Gap 差距2分";
                    wsScoreThan3.Name = "Gap 差距3分";
                    wsAverage.Name = "廠區核心職能分佈狀態";
                    
                    // fontsize mặc định cho cả sheet
                    wsAll.Cells.Style.Font.Size = 12;
                    ws.Cells.Style.Font.Size = 12;
                    wsScoreEquals2.Cells.Style.Font.Size = 12;
                    wsScoreThan2.Cells.Style.Font.Size = 12;
                    wsScoreThan3.Cells.Style.Font.Size = 12;
                    wsAverage.Cells.Style.Font.Size = 12;
                    
                    // font family mặc định cho cả sheet
                    wsAll.Cells.Style.Font.Name = "Calibri";
                    ws.Cells.Style.Font.Name = "Calibri";
                    wsScoreEquals2.Cells.Style.Font.Name = "Calibri";
                    wsScoreThan2.Cells.Style.Font.Name = "Calibri";
                    wsScoreThan3.Cells.Style.Font.Name = "Calibri";
                    wsAverage.Cells.Style.Font.Name = "Calibri";

                    //////////// Sheet 1 //////////

                    var headersAll = new string[]{
                        "#", "Core Competencies", "Behavior indicator",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexAll = 1;
                    int headerColIndexAll = 1;
                    foreach (var header in headersAll)
                    {
                        int col = headerColIndexAll++;
                        wsAll.Cells[headerRowIndexAll, col].Value = header;
                        wsAll.Cells[headerRowIndexAll, col].Style.Font.Bold = true;
                        wsAll.Cells[headerRowIndexAll, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexAll = 1;
                    int rowIndexAll = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listAllCoreCompetencies)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexAll = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexAll++;

                        //gán giá trị cho từng cell   
                                        
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Index;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.AttHeading;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.AttBehavior;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Factory;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Center;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Dept;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.L1;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.L2;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Fl;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.L0;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.ScoreBy;
                        wsAll.Cells[rowIndexAll, colIndexAll++].Value = body.Score;
                        
                    }

                    wsAll.Cells[wsAll.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsAll.Cells[wsAll.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsAll.Cells[wsAll.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsAll.Cells[wsAll.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersAll.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsAll.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsAll.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsAll.Column(col).AutoFit();
                    }    

                    ///////////// Sheet 2 //////////////

                    var headersScoreEquals2 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser",
                        "Score", "Comment"
                    };

                    int headerRowIndexScoreEquals2 = 1;
                    int headerColIndexScoreEquals2 = 1;
                    foreach (var header in headersScoreEquals2)
                    {
                        int col = headerColIndexScoreEquals2++;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Value = header;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Style.Font.Bold = true;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreEquals2 = 1;
                    int rowIndexScoreEquals2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreEquals2)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreEquals2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreEquals2++;

                        //gán giá trị cho từng cell   
                                       
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Index;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.AttHeading;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Factory;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Center;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Dept;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L1;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L2;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Fl;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L0;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.ScoreBy;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Score;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Comment;
                        
                    }

                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreEquals2.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreEquals2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreEquals2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreEquals2.Column(col).AutoFit();
                    }

                    //////////// Sheet 3 //////////

                    var headers = new string[]{
                        "#", "Core Competencies", "Behavior indicator",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser",
                        "Score"
                    };

                    int headerRowIndex = 1;
                    int headerColIndex = 1;
                    foreach (var header in headers)
                    {
                        int col = headerColIndex++;
                        ws.Cells[headerRowIndex, col].Value = header;
                        ws.Cells[headerRowIndex, col].Style.Font.Bold = true;
                        ws.Cells[headerRowIndex, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex = 1;
                    int rowIndex = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScore)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        //gán giá trị cho từng cell   
                                        
                        ws.Cells[rowIndex, colIndex++].Value = body.Index;
                        ws.Cells[rowIndex, colIndex++].Value = body.AttHeading;
                        ws.Cells[rowIndex, colIndex++].Value = body.AttBehavior;
                        ws.Cells[rowIndex, colIndex++].Value = body.Factory;
                        ws.Cells[rowIndex, colIndex++].Value = body.Center;
                        ws.Cells[rowIndex, colIndex++].Value = body.Dept;
                        ws.Cells[rowIndex, colIndex++].Value = body.L1;
                        ws.Cells[rowIndex, colIndex++].Value = body.L2;
                        ws.Cells[rowIndex, colIndex++].Value = body.Fl;
                        ws.Cells[rowIndex, colIndex++].Value = body.L0;
                        ws.Cells[rowIndex, colIndex++].Value = body.ScoreBy;
                        ws.Cells[rowIndex, colIndex++].Value = body.Score;
                        
                    }

                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersAll.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(col).AutoFit();
                    }

                    /////////// Sheet 4 ///////////

                    var headersScoreThan2 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Self-score", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexScoreThan2 = 1;
                    int headerColIndexScoreThan2 = 1;
                    foreach (var header in headersScoreThan2)
                    {
                        int col = headerColIndexScoreThan2++;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Value = header;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Style.Font.Bold = true;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreThan2 = 1;
                    int rowIndexScoreThan2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreThan2)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreThan2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreThan2++;

                        //gán giá trị cho từng cell   
                                        
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Index;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.AttHeading;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Factory;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Center;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Dept;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L1;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L2;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Fl;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L0;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.SelfScore;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.ScoreBy;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Score;
                        
                    }

                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreThan2.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreThan2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreThan2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreThan2.Column(col).AutoFit();
                    }    
                    
                    /////////// Sheet 5 ///////////
                    
                    var headersScoreThan3 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Self-score", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexScoreThan3 = 1;
                    int headerColIndexScoreThan3 = 1;
                    foreach (var header in headersScoreThan3)
                    {
                        int col = headerColIndexScoreThan3++;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Value = header;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Style.Font.Bold = true;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreThan3 = 1;
                    int rowIndexScoreThan3 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreThan3)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreThan3 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreThan3++;

                        //gán giá trị cho từng cell   
                                        
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Index;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.AttHeading;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Factory;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Center;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Dept;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L1;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L2;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Fl;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L0;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.SelfScore;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.ScoreBy;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Score;
                        
                    }

                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreThan3.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreThan3.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreThan3.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreThan3.Column(col).AutoFit();
                    }

                    ///////// Sheet 6 ////////////
                    
                    var headersAverage = new string[]{
                        "#", "Factory",
                        "Core Competencies", "Self score Avg", "L1 Avg",
                        "L2 Avg", "FL Avg",  "Fty Average"
                    };

                    int headerRowIndexAverage = 1;
                    int headerColIndexAverage = 1;
                    foreach (var header in headersAverage)
                    {
                        int col = headerColIndexAverage++;
                        wsAverage.Cells[headerRowIndexAverage, col].Value = header;
                        wsAverage.Cells[headerRowIndexAverage, col].Style.Font.Bold = true;
                        wsAverage.Cells[headerRowIndexAverage, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexAverage = 1;
                    int rowIndexAverage = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesAverage)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexAverage = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexAverage++;

                        //gán giá trị cho từng cell                      
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Index;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Factory;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.AttHeading;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L0Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L1Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L2Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.FLAverage;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.FtyAverage;
                        
                    }

                    headerColIndexAverage = 1;
                    rowIndexAverage += 2;

                    var headersPercentile = new string[]{
                        "#", "Factory",
                        "Core Competencies", "10th percentile", "25th percentile",
                        "50th percentile", "75th percentile",  "90th percentile"
                    };
                    
                    foreach (var header2 in headersPercentile)
                    {
                        int col = headerColIndexAverage++;
                        wsAverage.Cells[rowIndexAverage, col].Value = header2;
                        wsAverage.Cells[rowIndexAverage, col].Style.Font.Bold = true;
                        wsAverage.Cells[rowIndexAverage, col].Style.Font.Size = 12;
                    }
                    // end Style
                    
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesPercentile)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexAverage = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexAverage++;

                        //gán giá trị cho từng cell                      
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Index;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Factory;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.AttHeading;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P10;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P25;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P50;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P75;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P90;
                        
                    }

                    var indexRowSpace = listCoreCompetenciesAverage.Count() + 2;
                    
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersAverage.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsAverage.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsAverage.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsAverage.Column(col).AutoFit();

                        wsAverage.Cells[indexRowSpace, col].Style.Border.Top.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Right.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Bottom.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Left.Style = ExcelBorderStyle.None;
                    }

 
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.Write(mes);
                return new Byte[] { };
            }
        }
        
        public async Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreEquals2(string lang, int campaignID)
        {
            var index = 1;
            var queryAttitude = await _repoScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryL0 = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var result = (from a in queryAttitude
                        join b in queryL0 on a.ScoreTo equals b.Id
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLTW
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""
                        let l1 = _repo.FindById(b.Manager) != null ? _repo.FindById(b.Manager).FullName : ""
                        let l2 = _repo.FindById(b.L2) != null ? _repo.FindById(b.L2).FullName : ""
                        let fl = _repo.FindById(b.FunctionalLeader) != null ? _repo.FindById(b.FunctionalLeader).FullName : ""
                        let l0 = _repo.FindById(b.Id) != null ? _repo.FindById(b.Id).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreBy) != null ? _repo.FindById(a.ScoreBy).FullName : ""
                        let score = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Score : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Score : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Score : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FLScore : "0"
                        let comment = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Comment : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Comment : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Comment : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FlComment : ""
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let submited = attitudeSubmit != null ? _repo.FindById(attitudeSubmit.SubmitTo) != null ? _repo.FindById(attitudeSubmit.SubmitTo).Id == a.ScoreBy ? attitudeSubmit.BtnL0 : _repo.FindById(attitudeSubmit.SubmitTo).Manager.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL1 : _repo.FindById(attitudeSubmit.SubmitTo).L2.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL2 : _repo.FindById(attitudeSubmit.SubmitTo).FunctionalLeader.ToInt() == a.ScoreBy ? attitudeSubmit.BtnFL : true : true : true
                        select new CoreCompetenciesDto ()
                        {
                            Index = 0,
                            AttHeadingID = a.AttitudeHeadingID,
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            Score = score,
                            ScoreBy = scoreBy,
                            Comment = comment,
                            Submited = submited
                        }).Where(x => !x.Submited && x.Score == "2").OrderBy(x => x.L0).ThenBy(x => x.ScoreBy).ThenBy(x => x.AttHeadingID).ToList();

            result.ForEach(x => {
                x.Index = index;
                index++;
            });
            
            return result;
        }

        public async Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreThan2(string lang, int campaignID)
        {
            var index = 1;
            var queryAttitude = await _repoScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryL0 = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var result = (from a in queryAttitude
                        join b in queryL0 on a.ScoreTo equals b.Id
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLTW
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""
                        let l1 = _repo.FindById(b.Manager) != null ? _repo.FindById(b.Manager).FullName : ""
                        let l2 = _repo.FindById(b.L2) != null ? _repo.FindById(b.L2).FullName : ""
                        let fl = _repo.FindById(b.FunctionalLeader) != null ? _repo.FindById(b.FunctionalLeader).FullName : ""
                        let l0 = _repo.FindById(b.Id) != null ? _repo.FindById(b.Id).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreBy) != null ? _repo.FindById(a.ScoreBy).FullName : ""
                        let score = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Score : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Score : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Score : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FLScore : "0"
                        let comment = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Comment : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Comment : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Comment : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FlComment : ""
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let selfScore = queryAttitude.FirstOrDefault(x => x.AttitudeHeadingID == a.AttitudeHeadingID && x.ScoreTo == a.ScoreTo && x.ScoreBy == b.Id) != null ? attitudeSubmit != null ? !attitudeSubmit.BtnL0 ? queryAttitude.FirstOrDefault(x => x.AttitudeHeadingID == a.AttitudeHeadingID && x.ScoreTo == a.ScoreTo && x.ScoreBy == b.Id).L0Score : "N/A" : "N/A" : "N/A"
                        let submited = attitudeSubmit != null ? _repo.FindById(attitudeSubmit.SubmitTo) != null ? _repo.FindById(attitudeSubmit.SubmitTo).Id == a.ScoreBy ? attitudeSubmit.BtnL0 : _repo.FindById(attitudeSubmit.SubmitTo).Manager.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL1 : _repo.FindById(attitudeSubmit.SubmitTo).L2.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL2 : _repo.FindById(attitudeSubmit.SubmitTo).FunctionalLeader.ToInt() == a.ScoreBy ? attitudeSubmit.BtnFL : true : true : true
                        select new CoreCompetenciesDto ()
                        {
                            Index = 0,
                            AttHeadingID = a.AttitudeHeadingID,
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            SelfScore = selfScore,
                            Score = score,
                            ScoreBy = scoreBy,
                            Comment = comment,
                            Submited = submited
                        }).Where(x => !x.Submited && x.ScoreBy != x.L0 && ((x.SelfScore == "N/A" && x.Score == "2") || (x.SelfScore == "2" && x.Score == "4") || (x.SelfScore == "4" && x.Score == "2") || (x.SelfScore == "3" && x.Score == "5") || (x.SelfScore == "5" && x.Score == "3"))).OrderBy(x => x.L0).ThenBy(x => x.ScoreBy).ThenBy(x => x.AttHeadingID).ToList();

            result.ForEach(x => {
                x.Index = index;
                index++;
            });
            
            return result;
        }

        public async Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreThan3(string lang, int campaignID)
        {
            var index = 1;
            var queryAttitude = await _repoScore.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryL0 = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var result = (from a in queryAttitude
                        join b in queryL0 on a.ScoreTo equals b.Id
                        let attHeading = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == _repoAttitudeHeading.FindById(a.AttitudeHeadingID).Name).FirstOrDefault().SLTW
                        let factory = _repoOc.FindById(b.FactId) != null ? _repoOc.FindById(b.FactId).Name : ""
                        let center = _repoOc.FindById(b.CenterId) != null ? _repoOc.FindById(b.CenterId).Name : ""
                        let dept = _repoOc.FindById(b.DeptId) != null ? _repoOc.FindById(b.DeptId).Name : ""
                        let l1 = _repo.FindById(b.Manager) != null ? _repo.FindById(b.Manager).FullName : ""
                        let l2 = _repo.FindById(b.L2) != null ? _repo.FindById(b.L2).FullName : ""
                        let fl = _repo.FindById(b.FunctionalLeader) != null ? _repo.FindById(b.FunctionalLeader).FullName : ""
                        let l0 = _repo.FindById(b.Id) != null ? _repo.FindById(b.Id).FullName : ""
                        let scoreBy = _repo.FindById(a.ScoreBy) != null ? _repo.FindById(a.ScoreBy).FullName : ""
                        let score = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Score : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Score : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Score : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FLScore : "0"
                        let comment = a.ScoreBy == _repo.FindById(b.Id).Id ? a.L0Comment : a.ScoreBy == _repo.FindById(b.Id).Manager ? a.L1Comment : a.ScoreBy == _repo.FindById(b.Id).L2 ? a.L2Comment : a.ScoreBy == _repo.FindById(b.Id).FunctionalLeader ? a.FlComment : ""
                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                        let selfScore = queryAttitude.FirstOrDefault(x => x.AttitudeHeadingID == a.AttitudeHeadingID && x.ScoreTo == a.ScoreTo && x.ScoreBy == b.Id) != null ? attitudeSubmit != null ? !attitudeSubmit.BtnL0 ? queryAttitude.FirstOrDefault(x => x.AttitudeHeadingID == a.AttitudeHeadingID && x.ScoreTo == a.ScoreTo && x.ScoreBy == b.Id).L0Score : "N/A" : "N/A" : "N/A"
                        let submited = attitudeSubmit != null ? _repo.FindById(attitudeSubmit.SubmitTo) != null ? _repo.FindById(attitudeSubmit.SubmitTo).Id == a.ScoreBy ? attitudeSubmit.BtnL0 : _repo.FindById(attitudeSubmit.SubmitTo).Manager.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL1 : _repo.FindById(attitudeSubmit.SubmitTo).L2.ToInt() == a.ScoreBy ? attitudeSubmit.BtnL2 : _repo.FindById(attitudeSubmit.SubmitTo).FunctionalLeader.ToInt() == a.ScoreBy ? attitudeSubmit.BtnFL : true : true : true
                        select new CoreCompetenciesDto ()
                        {
                            Index = 0,
                            AttHeadingID = a.AttitudeHeadingID,
                            AttHeading = attHeading,
                            Factory = factory,
                            Center = center,
                            Dept = dept,
                            L1 = l1,
                            L2 = l2,
                            Fl = fl,
                            L0 = l0,
                            SelfScore = selfScore,
                            Score = score,
                            ScoreBy = scoreBy,
                            Comment = comment,
                            Submited = submited
                        }).Where(x => !x.Submited && x.ScoreBy != x.L0 && ((x.SelfScore == "N/A" && x.Score == "3") || (x.SelfScore == "2" && x.Score == "5") || (x.SelfScore == "5" && x.Score == "2"))).OrderBy(x => x.L0).ThenBy(x => x.ScoreBy).ThenBy(x => x.AttHeadingID).ToList();

            result.ForEach(x => {
                x.Index = index;
                index++;
            });

            return result;
        }

        public async Task<List<CoreCompetenciesAverageDto>> GetAllCoreCompetenciesAverage(string lang, int campaignID)
        {
            var index = 1;
            var queryAttHeading = await _repoAttitudeHeading.FindAll().ToListAsync();
            var scoreTo = _repoScore.FindAll(x => x.CampaignID == campaignID).FirstOrDefault().ScoreTo;
            // var totalScoreL0 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == 1).Select(x => new {x.L0Score});
            // double sum = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == 1).ToList().Sum(x => x.L0Score.ToInt());
            // double hhh = _repoScore.FindAll().Where(x => x.ID == 568).FirstOrDefault().L0Score.ToInt();
            // double ggg = _repoScore.FindAll(x => x.ID == 568).ToList().Sum(x => x.L0Score.ToInt());

            double totalL1 = _repoAc.FindAll(x => x.Manager.Value > 0).Select(x => x.Manager).Distinct().ToList().Count();
            double totalL2 = _repoAc.FindAll(x => x.Manager.Value > 0).Select(x => x.L2).Distinct().ToList().Count();
            double totalFL = _repoAc.FindAll(x => x.Manager.Value > 0).Select(x => x.Manager).Distinct().ToList().Count();
            var data =  (from a in queryAttHeading
                         let factory = _repoOc.FindById(_repoAc.FindById(scoreTo).FactId).Name
                         let attHeadingName = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLTW
                         let countL0 = _repoAc.FindAll(x => x.L0.Value).Count()

                         select new CoreCompetenciesAverageDto () {
                            Index = a.ID.ToString(),
                            Factory = factory,
                            AttHeading = attHeadingName,
                            L0Average = AverageL0(a.ID, campaignID).Item3,
                            L1Average = AverageL1(a.ID, campaignID).Item3,
                            L2Average = AverageL2(a.ID, campaignID).Item3,
                            FLAverage = AverageFL(a.ID, campaignID).Item3,
                            FtyAverage = String.Format("{0:0.0}", Math.Round(((AverageL0(a.ID, campaignID).Item1 + AverageL1(a.ID, campaignID).Item1 + AverageL2(a.ID, campaignID).Item1 + AverageFL(a.ID, campaignID).Item1)/(AverageL0(a.ID, campaignID).Item2 + AverageL1(a.ID, campaignID).Item2 + AverageL2(a.ID, campaignID).Item2 + AverageFL(a.ID, campaignID).Item2)), 1))
                         }).ToList();

            data.ForEach(x => {
                x.Index = index.ToString();
                index++;
            });
            
            var item = new CoreCompetenciesAverageDto() {
                            Index = null,
                            Factory = data.FirstOrDefault().Factory,
                            AttHeading = lang == SystemLang.EN ? "Total score" : "總分",
                            L0Average = String.Format("{0:0.0}",data.Sum(x => x.L0Average.ToDouble())),
                            L1Average = String.Format("{0:0.0}",data.Sum(x => x.L1Average.ToDouble())),
                            L2Average = String.Format("{0:0.0}",data.Sum(x => x.L2Average.ToDouble())),
                            FLAverage = String.Format("{0:0.0}",data.Sum(x => x.FLAverage.ToDouble())),
                            FtyAverage = String.Format("{0:0.0}",data.Sum(x => x.FtyAverage.ToDouble()))
            };
            data.Add(item);

            return data;
        }

        // public string AverageL0 (int attHeading, int campaignID) {
        //     double totalL0 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L0Score != null && x.L0Score != "")).Count();
        //     double totalScoreL0 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L0Score != null || x.L0Score != "")).ToList().Sum(x => x.L0Score.ToInt());
        //     double average = totalScoreL0 / totalL0;
        //     average = Math.Ceiling(average);
        //     return String.Format("{0:0.0}", average);
        // }

        private Tuple<double, double, string> AverageL0(int attHeading, int campaignID) {
            var listScoreL0 = (from a in _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L0Score != null && x.L0Score != "")).ToList()
                                let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                                let submited = attitudeSubmit != null ? attitudeSubmit.BtnL0 : true
                                select new {
                                    Score = a.L0Score,
                                    Submited = submited
                                }).Where(x => !x.Submited).ToList();
            double totalL0 = listScoreL0.Count();
            double totalScoreL0 = listScoreL0.Sum(x => x.Score.ToInt());
            // double totalL0 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L0Score != null && x.L0Score != "")).Count();
            // double totalScoreL0 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L0Score != null || x.L0Score != "")).ToList().Sum(x => x.L0Score.ToInt());
            double average = totalScoreL0 / totalL0;
            // average = Math.Ceiling(average);
            average = Math.Round(average, 1);
            return Tuple.Create(totalScoreL0, totalL0, String.Format("{0:0.0}", average)); 
        }

        private Tuple<double, double, string> AverageL1(int attHeading, int campaignID) { 
            var listScoreL1 = (from a in _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L1Score != null && x.L1Score != "")).ToList()
                                let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                                let submited = attitudeSubmit != null ? attitudeSubmit.BtnL1 : true
                                select new {
                                    Score = a.L1Score,
                                    Submited = submited
                                }).Where(x => !x.Submited).ToList();
            double totalL1 = listScoreL1.Count();
            double totalScoreL1 = listScoreL1.Sum(x => x.Score.ToInt());
            
            // double totalScoreL1 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L1Score != null || x.L1Score != "")).ToList().Sum(x => x.L1Score.ToInt());
            double average = totalScoreL1 / totalL1;
            // average = Math.Ceiling(average);
            average = Math.Round(average, 1);
            return Tuple.Create(totalScoreL1, totalL1, String.Format("{0:0.0}", average)); 
        }

        private Tuple<double, double, string> AverageL2(int attHeading, int campaignID) { 
            var listScoreL2 = (from a in _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L2Score != null && x.L2Score != "")).ToList()
                                let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                                let submited = attitudeSubmit != null ? attitudeSubmit.BtnL2 : true
                                select new {
                                    Score = a.L2Score,
                                    Submited = submited
                                }).Where(x => !x.Submited).ToList();
            double totalL2 = listScoreL2.Count();
            double totalScoreL2 = listScoreL2.Sum(x => x.Score.ToInt());
            
            // double totalScoreL2 = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.L2Score != null || x.L2Score != "")).ToList().Sum(x => x.L2Score.ToInt());
            double average = totalScoreL2 / totalL2;
            // average = Math.Ceiling(average);
            average = Math.Round(average, 1);
            return Tuple.Create(totalScoreL2, totalL2, String.Format("{0:0.0}", average)); 
        }

        private Tuple<double, double, string> AverageFL(int attHeading, int campaignID) { 
            var listScoreFL = (from a in _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.FLScore != null && x.FLScore != "")).ToList()
                                let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                                let submited = attitudeSubmit != null ? attitudeSubmit.BtnFL : true
                                select new {
                                    Score = a.FLScore,
                                    Submited = submited
                                }).Where(x => !x.Submited).ToList();
            double totalFL = listScoreFL.Count();
            double totalScoreFL = listScoreFL.Sum(x => x.Score.ToInt());
            
            // double totalScoreFL = _repoScore.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attHeading && (x.FLScore != null || x.FLScore != "")).ToList().Sum(x => x.FLScore.ToInt());
            double average = totalScoreFL / totalFL;
            // average = Math.Ceiling(average);
            average = Math.Round(average, 1);
            return Tuple.Create(totalScoreFL, totalFL, String.Format("{0:0.0}", average)); 
        }

        
        public async Task<List<CoreCompetenciesPercentileDto>> GetAllCoreCompetenciesPercentile(string lang, int campaignID)
        {
            var index = 1;
            var queryAttHeading = await _repoAttitudeHeading.FindAll().ToListAsync();
            var queryL0Score = (from a in _repoScore.FindAll(x => x.CampaignID == campaignID && Convert.ToInt32(x.L0Score) > 0).ToList()
                                        let attitudeSubmit = _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.SubmitTo == a.ScoreTo).FirstOrDefault()
                                        let submited = attitudeSubmit != null ?  attitudeSubmit.BtnL0 : true
                                        select new {
                                            AttitudeHeadingID = a.AttitudeHeadingID,
                                            ScoreTo = a.ScoreTo,
                                            L0Score = a.L0Score,
                                            Submited = submited
                                        }).ToList();
            var scoreTo = _repoScore.FindAll(x => x.CampaignID == campaignID).FirstOrDefault().ScoreTo;

            var data =  (from a in queryAttHeading
                         let factory = _repoOc.FindById(_repoAc.FindById(scoreTo).FactId).Name
                         let attHeadingName = lang == SystemLang.EN ? _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLEN : _repoSystemLanguage.FindAll(x => x.SLKey == a.Name).FirstOrDefault().SLTW
                         let countL0 = queryL0Score.FindAll(x => x.AttitudeHeadingID == a.ID && !x.Submited).Count().ToDouble()
                         let p10 = Math.Ceiling((countL0 * 10/100).ToDouble()) - 1
                         let p25 = Math.Ceiling((countL0 * 25/100).ToDouble()) - 1
                         let p50 = Math.Ceiling((countL0 * 50/100).ToDouble()) - 1
                         let p75 = Math.Ceiling((countL0 * 75/100).ToDouble()) - 1
                         let p90 = Math.Ceiling((countL0 * 90/100).ToDouble()) - 1

                         let p10Score = queryL0Score.Where(x => x.AttitudeHeadingID == a.ID && !x.Submited).OrderBy(x => x.L0Score).ThenBy(x => x.ScoreTo).ToList()[p10.ToInt()].L0Score
                         let p25Score = queryL0Score.Where(x => x.AttitudeHeadingID == a.ID && !x.Submited).OrderBy(x => x.L0Score).ThenBy(x => x.ScoreTo).ToList()[p25.ToInt()].L0Score
                         let p50Score = queryL0Score.Where(x => x.AttitudeHeadingID == a.ID && !x.Submited).OrderBy(x => x.L0Score).ThenBy(x => x.ScoreTo).ToList()[p50.ToInt()].L0Score
                         let p75Score = queryL0Score.Where(x => x.AttitudeHeadingID == a.ID && !x.Submited).OrderBy(x => x.L0Score).ThenBy(x => x.ScoreTo).ToList()[p75.ToInt()].L0Score
                         let p90Score = queryL0Score.Where(x => x.AttitudeHeadingID == a.ID && !x.Submited).OrderBy(x => x.L0Score).ThenBy(x => x.ScoreTo).ToList()[p90.ToInt()].L0Score
                         
                         select new CoreCompetenciesPercentileDto () {
                            Index = a.ID.ToString(),
                            Factory = factory,
                            AttHeading = attHeadingName,
                            P10 = String.Format("{0:0.0}", p10Score.ToDouble()),
                            P25 = String.Format("{0:0.0}", p25Score.ToDouble()),
                            P50 = String.Format("{0:0.0}", p50Score.ToDouble()),
                            P75 = String.Format("{0:0.0}", p75Score.ToDouble()),
                            P90 = String.Format("{0:0.0}", p90Score.ToDouble())
                         }).ToList();

            data.ForEach(x => {
                x.Index = index.ToString();
                index++;
            });
            
            var item = new CoreCompetenciesPercentileDto() {
                            Index = null,
                            Factory = data.FirstOrDefault().Factory,
                            AttHeading = lang == SystemLang.EN ? "Total score" : "總分",
                            P10 = String.Format("{0:0.0}",data.Sum(x => x.P10.ToDouble())),
                            P25 = String.Format("{0:0.0}",data.Sum(x => x.P25.ToDouble())),
                            P50 = String.Format("{0:0.0}",data.Sum(x => x.P50.ToDouble())),
                            P75 = String.Format("{0:0.0}",data.Sum(x => x.P75.ToDouble())),
                            P90 = String.Format("{0:0.0}",data.Sum(x => x.P90.ToDouble()))
            };
            data.Add(item);

            return data;
        }

        public async Task<List<CoreCompetenciesAttitudeBehaviorDto>> GetAllCoreCompetenciesAttitudeBehavior(string lang, int campaignID)
        {
            var index = 1;
            var queryL0 = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var querySystemLanguage = await _repoSystemLanguage.FindAll().ToListAsync();
            var queryBehaviorCheck = await _repoBehaviorCheck.FindAll(x => x.CampaignID == campaignID && (x.L0Checked || x.L1Checked || x.L2Checked || x.FLChecked)).ToListAsync();
            var queryAttHeading = (from a in _repoAttitudeHeading.FindAll().ToList()
                                    join b in querySystemLanguage on a.Name equals b.SLKey
                                    select new {
                                        ID = a.ID,
                                        Name = lang == SystemLang.EN ? b.SLEN : b.SLTW
                                    }).ToList();
            
            var queryCategory = (from a in _repoAttCategory.FindAll(x => x.CampaignID == campaignID).ToList()
                                    join b in querySystemLanguage on a.Name equals b.SLKey
                                    select new {
                                        ID = a.ID,
                                        Name = lang == SystemLang.EN ? b.SLEN : b.SLTW
                                    }).ToList();

            var queryBehavior = (from a in queryBehaviorCheck
                                join b in await _repoAttBehavior.FindAll().ToListAsync() on a.BehaviorID equals b.ID into ab
                                from c in ab
                                join d in querySystemLanguage on c.Name equals d.SLKey into cd
                                from e in cd
                                let behaviorName = lang == SystemLang.EN ? e.SLEN : e.SLTW
                                join f in await _repoAttKeypoint.FindAll().ToListAsync() on c.AttitudeKeypointID equals f.ID into cf
                                from g in cf
                                join h in querySystemLanguage on g.Name equals h.SLKey into gh
                                from i in gh
                                let keypointName = lang == SystemLang.EN ? i.SLEN : i.SLTW
                                let categoryName = queryCategory.FindAll(x => x.ID == g.AttitudeCategoryID).FirstOrDefault().Name
                                let headingName = queryAttHeading.FindAll(x => x.ID == g.AttitudeHeadingID).FirstOrDefault().Name
                                
                                join j in queryL0 on a.CheckTo equals j.Id
                                let factory = _repoOc.FindById(j.FactId) != null ? _repoOc.FindById(j.FactId).Name : ""
                                let center = _repoOc.FindById(j.CenterId) != null ? _repoOc.FindById(j.CenterId).Name : ""
                                let dept = _repoOc.FindById(j.DeptId) != null ? _repoOc.FindById(j.DeptId).Name : ""
                                let l1 = _repo.FindById(j.Manager) != null ? _repo.FindById(j.Manager).FullName : ""
                                let l2 = _repo.FindById(j.L2) != null ? _repo.FindById(j.L2).FullName : ""
                                let fl = _repo.FindById(j.FunctionalLeader) != null ? _repo.FindById(j.FunctionalLeader).FullName : ""
                                let l0 = _repo.FindById(j.Id) != null ? _repo.FindById(j.Id).FullName : ""
                                let checkBy = _repo.FindById(a.CheckBy) != null ? _repo.FindById(a.CheckBy).FullName : ""
                                
                                select new CoreCompetenciesAttitudeBehaviorDto () {
                                    Index = 0,
                                    AttHeadingID =  g.AttitudeHeadingID,
                                    AttHeading = headingName,
                                    L1 = l1,
                                    L2 = l2,
                                    Fl = fl,
                                    L0 = l0,
                                    CheckBy = checkBy,
                                    Category = categoryName,
                                    Keypoint = keypointName,
                                    Level = g.Level.ToString(),
                                    Tick = "✔",
                                    Center = center,
                                    Factory = factory,
                                    Dept = dept,
                                    Behavior = behaviorName,
                                }).OrderBy(x => x.L0).ThenBy(x => x.CheckBy).ThenBy(x => x.AttHeadingID).ThenByDescending(x => x.Level).ToList();

            queryBehavior.ForEach(x => {
                x.Index = index;
                index++;
            });

            return queryBehavior;
        }


        public async Task<byte[]> ExportExcelCoreCompetencies(string lang, int campaignID)
        {
            return await ExportExcelConsumptionCase1(lang, campaignID);
            //throw new NotImplementedException();
        }

        private async Task<Byte[]> ExportExcelConsumptionCase1(string lang, int campaignID)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Huu Quynh";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "CoreCompetencies";

                    var listCoreCompetencies = new List<CoreCompetenciesDto>();
                    listCoreCompetencies = await GetAllCoreCompetencies(lang, campaignID);

                    var listCoreCompetenciesScoreEquals2 = new List<CoreCompetenciesDto>();
                    listCoreCompetenciesScoreEquals2 = await GetAllCoreCompetenciesScoreEquals2(lang, campaignID);

                    var listCoreCompetenciesScoreThan2 = new List<CoreCompetenciesDto>();
                    listCoreCompetenciesScoreThan2 = await GetAllCoreCompetenciesScoreThan2(lang, campaignID);

                    var listCoreCompetenciesScoreThan3 = new List<CoreCompetenciesDto>();
                    listCoreCompetenciesScoreThan3 = await GetAllCoreCompetenciesScoreThan3(lang, campaignID);
                    
                    var listCoreCompetenciesAverage = new List<CoreCompetenciesAverageDto>();
                    listCoreCompetenciesAverage = await GetAllCoreCompetenciesAverage(lang, campaignID);
                    
                    var listCoreCompetenciesPercentile = new List<CoreCompetenciesPercentileDto>();
                    listCoreCompetenciesPercentile = await GetAllCoreCompetenciesPercentile(lang, campaignID);

                    var listCoreCompetenciesAttitudeBehavior = new List<CoreCompetenciesAttitudeBehaviorDto>();
                    listCoreCompetenciesAttitudeBehavior = await GetAllCoreCompetenciesAttitudeBehavior(lang, campaignID);
                    
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("CoreCompetencies");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreEquals2");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreThan2");
                    p.Workbook.Worksheets.Add("CoreCompetenciesScoreThan3");
                    p.Workbook.Worksheets.Add("CoreCompetenciesAverage");
                    p.Workbook.Worksheets.Add("CoreCompetenciesBehavior");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["CoreCompetencies"];
                    ExcelWorksheet wsScoreEquals2 = p.Workbook.Worksheets["CoreCompetenciesScoreEquals2"];
                    ExcelWorksheet wsScoreThan2 = p.Workbook.Worksheets["CoreCompetenciesScoreThan2"];
                    ExcelWorksheet wsScoreThan3 = p.Workbook.Worksheets["CoreCompetenciesScoreThan3"];
                    ExcelWorksheet wsAverage = p.Workbook.Worksheets["CoreCompetenciesAverage"];
                    ExcelWorksheet wsBehavior = p.Workbook.Worksheets["CoreCompetenciesBehavior"];


                    // đặt tên cho sheet
                    ws.Name = "核心職能資料分析";
                    wsScoreEquals2.Name = "核心職能評2分項目";
                    wsScoreThan2.Name = "Gap 差距2分";
                    wsScoreThan3.Name = "Gap 差距3分";
                    wsAverage.Name = "廠區核心職能分佈狀態";
                    wsBehavior.Name = "行為細項勾選狀態";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 12;
                    wsScoreEquals2.Cells.Style.Font.Size = 12;
                    wsScoreThan2.Cells.Style.Font.Size = 12;
                    wsScoreThan3.Cells.Style.Font.Size = 12;
                    wsAverage.Cells.Style.Font.Size = 12;
                    wsBehavior.Cells.Style.Font.Size = 12;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";
                    wsScoreEquals2.Cells.Style.Font.Name = "Calibri";
                    wsScoreThan2.Cells.Style.Font.Name = "Calibri";
                    wsScoreThan3.Cells.Style.Font.Name = "Calibri";
                    wsAverage.Cells.Style.Font.Name = "Calibri";
                    wsBehavior.Cells.Style.Font.Name = "Calibri";

                    //////////// Sheet 1 //////////

                    var headers = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser",
                        "Score", "Comment"
                    };

                    int headerRowIndex = 1;
                    int headerColIndex = 1;
                    foreach (var header in headers)
                    {
                        int col = headerColIndex++;
                        ws.Cells[headerRowIndex, col].Value = header;
                        ws.Cells[headerRowIndex, col].Style.Font.Bold = true;
                        ws.Cells[headerRowIndex, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex = 1;
                    int rowIndex = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetencies)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        //gán giá trị cho từng cell   
                                        
                        ws.Cells[rowIndex, colIndex++].Value = body.Index;
                        ws.Cells[rowIndex, colIndex++].Value = body.AttHeading;
                        ws.Cells[rowIndex, colIndex++].Value = body.Factory;
                        ws.Cells[rowIndex, colIndex++].Value = body.Center;
                        ws.Cells[rowIndex, colIndex++].Value = body.Dept;
                        ws.Cells[rowIndex, colIndex++].Value = body.L1;
                        ws.Cells[rowIndex, colIndex++].Value = body.L2;
                        ws.Cells[rowIndex, colIndex++].Value = body.Fl;
                        ws.Cells[rowIndex, colIndex++].Value = body.L0;
                        ws.Cells[rowIndex, colIndex++].Value = body.ScoreBy;
                        ws.Cells[rowIndex, colIndex++].Value = body.Score;
                        ws.Cells[rowIndex, colIndex++].Value = body.Comment;
                        
                    }

                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headers.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(col).AutoFit();
                    }    

                    ///////////// Sheet 2 //////////////

                    var headersScoreEquals2 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexScoreEquals2 = 1;
                    int headerColIndexScoreEquals2 = 1;
                    foreach (var header in headersScoreEquals2)
                    {
                        int col = headerColIndexScoreEquals2++;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Value = header;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Style.Font.Bold = true;
                        wsScoreEquals2.Cells[headerRowIndexScoreEquals2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreEquals2 = 1;
                    int rowIndexScoreEquals2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreEquals2)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreEquals2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreEquals2++;

                        //gán giá trị cho từng cell   
                                       
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Index;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.AttHeading;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Factory;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Center;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Dept;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L1;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L2;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Fl;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.L0;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.ScoreBy;
                        wsScoreEquals2.Cells[rowIndexScoreEquals2, colIndexScoreEquals2++].Value = body.Score;
                        
                    }

                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreEquals2.Cells[wsScoreEquals2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreEquals2.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreEquals2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreEquals2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreEquals2.Column(col).AutoFit();
                    }

                    /////////// Sheet 3 ///////////

                    var headersScoreThan2 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Self-score", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexScoreThan2 = 1;
                    int headerColIndexScoreThan2 = 1;
                    foreach (var header in headersScoreThan2)
                    {
                        int col = headerColIndexScoreThan2++;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Value = header;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Style.Font.Bold = true;
                        wsScoreThan2.Cells[headerRowIndexScoreThan2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreThan2 = 1;
                    int rowIndexScoreThan2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreThan2)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreThan2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreThan2++;

                        //gán giá trị cho từng cell   
                                        
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Index;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.AttHeading;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Factory;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Center;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Dept;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L1;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L2;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Fl;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.L0;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.SelfScore;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.ScoreBy;
                        wsScoreThan2.Cells[rowIndexScoreThan2, colIndexScoreThan2++].Value = body.Score;
                        
                    }

                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreThan2.Cells[wsScoreThan2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreThan2.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreThan2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreThan2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreThan2.Column(col).AutoFit();
                    }    
                    
                    /////////// Sheet 4 ///////////
                    
                    var headersScoreThan3 = new string[]{
                        "#", "Core Competencies",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Self-score", "Appraiser",
                        "Score"
                    };

                    int headerRowIndexScoreThan3 = 1;
                    int headerColIndexScoreThan3 = 1;
                    foreach (var header in headersScoreThan3)
                    {
                        int col = headerColIndexScoreThan3++;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Value = header;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Style.Font.Bold = true;
                        wsScoreThan3.Cells[headerRowIndexScoreThan3, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexScoreThan3 = 1;
                    int rowIndexScoreThan3 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesScoreThan3)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexScoreThan3 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexScoreThan3++;

                        //gán giá trị cho từng cell   
                                        
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Index;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.AttHeading;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Factory;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Center;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Dept;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L1;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L2;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Fl;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.L0;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.SelfScore;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.ScoreBy;
                        wsScoreThan3.Cells[rowIndexScoreThan3, colIndexScoreThan3++].Value = body.Score;
                        
                    }

                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsScoreThan3.Cells[wsScoreThan3.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersScoreThan3.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsScoreThan3.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsScoreThan3.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsScoreThan3.Column(col).AutoFit();
                    }

                    ///////// Sheet 5 ////////////
                    
                    var headersAverage = new string[]{
                        "#", "Factory",
                        "Core Competencies", "Self score Avg", "L1 Avg",
                        "L2 Avg", "FL Avg",  "Fty Average"
                    };

                    int headerRowIndexAverage = 1;
                    int headerColIndexAverage = 1;
                    foreach (var header in headersAverage)
                    {
                        int col = headerColIndexAverage++;
                        wsAverage.Cells[headerRowIndexAverage, col].Value = header;
                        wsAverage.Cells[headerRowIndexAverage, col].Style.Font.Bold = true;
                        wsAverage.Cells[headerRowIndexAverage, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexAverage = 1;
                    int rowIndexAverage = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesAverage)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexAverage = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexAverage++;

                        //gán giá trị cho từng cell                      
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Index;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Factory;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.AttHeading;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L0Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L1Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.L2Average;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.FLAverage;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.FtyAverage;
                        
                    }

                    headerColIndexAverage = 1;
                    rowIndexAverage += 2;

                    var headersPercentile = new string[]{
                        "#", "Factory",
                        "Core Competencies", "10th percentile", "25th percentile",
                        "50th percentile", "75th percentile",  "90th percentile"
                    };
                    
                    foreach (var header2 in headersPercentile)
                    {
                        int col = headerColIndexAverage++;
                        wsAverage.Cells[rowIndexAverage, col].Value = header2;
                        wsAverage.Cells[rowIndexAverage, col].Style.Font.Bold = true;
                        wsAverage.Cells[rowIndexAverage, col].Style.Font.Size = 12;
                    }
                    // end Style
                    
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesPercentile)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexAverage = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexAverage++;

                        //gán giá trị cho từng cell                      
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Index;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.Factory;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.AttHeading;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P10;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P25;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P50;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P75;
                        wsAverage.Cells[rowIndexAverage, colIndexAverage++].Value = body.P90;
                        
                    }

                    var indexRowSpace = listCoreCompetenciesAverage.Count() + 2;
                    
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsAverage.Cells[wsAverage.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersAverage.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsAverage.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsAverage.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsAverage.Column(col).AutoFit();

                        wsAverage.Cells[indexRowSpace, col].Style.Border.Top.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Right.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Bottom.Style = ExcelBorderStyle.None;
                        wsAverage.Cells[indexRowSpace, col].Style.Border.Left.Style = ExcelBorderStyle.None;
                    }

                    //////////// Sheet 6 //////////

                    var headersBehavior = new string[]{
                        "#",
                        "Factory", "Center", "Dept",
                        "L1 manager", "L2 manager",  "FL", "Appraisee", "Appraiser", "Core Competencies",
                        "Category", "Level", "Key points", "Tick", "Observable behavior indicators"
                    };

                    int headerRowIndexBehavior = 1;
                    int headerColIndexBehavior = 1;
                    foreach (var header in headersBehavior)
                    {
                        int col = headerColIndexBehavior++;
                        wsBehavior.Cells[headerRowIndexBehavior, col].Value = header;
                        wsBehavior.Cells[headerRowIndexBehavior, col].Style.Font.Bold = true;
                        wsBehavior.Cells[headerRowIndexBehavior, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndexBehavior = 1;
                    int rowIndexBehavior = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listCoreCompetenciesAttitudeBehavior)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndexBehavior = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndexBehavior++;

                        //gán giá trị cho từng cell   
                                        
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Index;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Factory;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Center;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Dept;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.L1;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.L2;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Fl;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.L0;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.CheckBy;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.AttHeading;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Category;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Level;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Keypoint;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Tick;
                        wsBehavior.Cells[rowIndexBehavior, colIndexBehavior++].Value = body.Behavior;
                        
                    }

                    wsBehavior.Cells[wsBehavior.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsBehavior.Cells[wsBehavior.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsBehavior.Cells[wsBehavior.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsBehavior.Cells[wsBehavior.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headersBehavior.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        wsBehavior.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsBehavior.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsBehavior.Column(col).AutoFit();
                    }  
 
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.Write(mes);
                return new Byte[] { };
            }
        }
        
    }
}
