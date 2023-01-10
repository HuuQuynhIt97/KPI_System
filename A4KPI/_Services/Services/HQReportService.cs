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

namespace A4KPI._Services.Services
{

    public class HQReportService : IHQReportService
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
        private readonly IAccountCampaignRepository _repoAccountCampaign;
        private readonly INewAttitudeScoreRepository _repoNewAttitudeScore;
        private readonly IJobTitleRepository _repoJobTitle;
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
        public HQReportService(
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
            IAccountCampaignRepository repoAccountCampaign,
            INewAttitudeScoreRepository repoNewAttitudeScore,
            IJobTitleRepository repoJobTitle,
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
            _repoAccountCampaign = repoAccountCampaign;
            _repoNewAttitudeScore = repoNewAttitudeScore;
            _currentEnvironment = currentEnvironment;
            _repoJobTitle = repoJobTitle; 
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
        private object GetListLabel(int campaignID)
        {
            List<string> listLabels = new List<string>();
            List<int> listLabel = new List<int>();

            var campaign = _repoCampaign.FindById(campaignID);
            var start = campaign.StartMonth;
            var end = campaign.EndMonth;

            for (int i = start; i <= end; i++)
            {
                listLabel.Add(i);
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

            for (int i = start; i <= end; i++)
            {
                listLabel.Add(i);
            }
            foreach (var item in listLabel)
            {
                var result = _repoDo.FindAll(x => x.CreatedTime.Month == item && x.ActionId == actionID).FirstOrDefault();
                if (result != null)
                {
                    listItems.Add(result.Achievement);
                }
                else
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
            for (int i = start; i <= end; i++)
            {
                listLabel.Add(i);
            }
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
            for (int i = start; i <= end; i++)
            {
                listLabel.Add(i);
            }
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
                item = GetListItem(x.Id, campaignID)
            });


            return result;
        }
        public async Task<ListMutiOrPersionKPIAcDto> CheckMutiOrPersonKPI(int accountID)
        {
            var queryKpiAc = await _repoKPIAc.FindAll(x => x.AccountId == accountID).ToListAsync();
            var list_KpiAc = (from a in queryKpiAc
                              join b in _repoKPINew.FindAll(x => !x.IsDisplayTodo) on a.KpiId equals b.Id
                              select new MutiOrPersionKPIAcDto
                              {
                                  KpiId = a.KpiId,
                              }).ToList();

            var list_PersonalKpi = new List<MutiOrPersionKPIAcDto>();
            var list_MultiKpi = new List<MutiOrPersionKPIAcDto>();

            foreach (var item in list_KpiAc)
            {
                var query = _repoKPIAc.FindAll(x => x.KpiId == item.KpiId).ToList();
                if (query.Count() > 1)
                {
                    list_MultiKpi.Add(item);
                }
                else
                {
                    list_PersonalKpi.Add(item);
                }
            }
            return new ListMutiOrPersionKPIAcDto
            {
                personal = list_PersonalKpi,
                muti = list_MultiKpi
            };
        }
        public async Task<object> GetKPIDefaultPerson(int campaignID, int userID)
        {
            var current_year = DateTime.Now.Year;
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();

            //check kpi person or muti
            var list_kpiAc = CheckMutiOrPersonKPI(userID);

            //tra ve ket qua
            var result = (from x in list_kpiAc.Result.personal
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

        public async Task<object> GetKPIStringPerson(int campaignID, int userID)
        {
            var current_year = DateTime.Now.Year;
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();
            //check kpi person or muti
            var list_kpiAc = CheckMutiOrPersonKPI(userID);

            //tra ve ket qua
            var result = (from x in list_kpiAc.Result.personal
                          join y in list_kpi on x.KpiId equals y.Id
                          join z in list_type on y.TypeId equals z.Id
                          select new
                          {
                              y.Id,
                              y.Name,
                              y.Year,
                              labels = GetListLabel(campaignID),
                              kpiType = z.Description,
                              data = GetListDataString(y.Id, campaignID, userID)
                          }).Where(x => x.kpiType == "string" && x.Year == current_year.ToString());
            return result;
        }

        public async Task<object> GetKPIDefaultMuti(int campaignID, int userID)
        {
            var current_year = DateTime.Now.Year;
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();

            //check kpi person or muti
            var list_kpiAc = CheckMutiOrPersonKPI(userID);

            //tra ve ket qua
            var result = (from x in list_kpiAc.Result.muti
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

        public async Task<object> GetKPIStringMuti(int campaignID, int userID)
        {
            var current_year = DateTime.Now.Year;
            var list_kpi = await _repoKPINew.FindAll().ToListAsync();
            var list_type = await _repoType.FindAll().ToListAsync();
            //check kpi person or muti
            var list_kpiAc = CheckMutiOrPersonKPI(userID);

            //tra ve ket qua
            var result = (from x in list_kpiAc.Result.muti
                          join y in list_kpi on x.KpiId equals y.Id
                          join z in list_type on y.TypeId equals z.Id
                          select new
                          {
                              y.Id,
                              y.Name,
                              y.Year,
                              labels = GetListLabel(campaignID),
                              kpiType = z.Description,
                              data = GetListDataString(y.Id, campaignID, userID)
                          }).Where(x => x.kpiType == "string" && x.Year == current_year.ToString());
            return result;
        }


        public async Task<List<PeopleCommitteeDto>> GetAll(string lang, int campaignID)
        {
            var index = 1;
            var queryEvalution = await _repoEvaluation.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryChild = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var queryParent = await _repo.FindAll().ToListAsync();
            var model = (from a in queryChild
                        join b in queryEvalution on a.Id equals b.UserID
                        let manager = _repo.FindById(a.Manager) != null ? _repo.FindById(a.Manager).FullName : "N/A"
                        let center = _repoOc.FindById(a.CenterId) != null ? _repoOc.FindById(a.CenterId).Name : "N/A"
                        let dept = _repoOc.FindById(a.DeptId) != null ? _repoOc.FindById(a.DeptId).Name : "N/A"
                        select new PeopleCommitteeDto
                        {
                            AppraiseeID = a.Id,
                            L1ID = a.Manager.Value,
                            Appraisee = a.FullName,
                            L1Manager = manager,
                            Center = center,
                            Dept = dept,
                            Index = index
                        }).ToList();

            model.ForEach(item => {
                item.Index = index;
                index++;
            });

            var list_CommitteeSequence = new List<CommitteeSequence>();

            foreach (var item in model)
            {
                var committeeSequence = new CommitteeSequence();
                var queryCmteeSequence = _repoCommitteeSequence.FindAll(x => x.CampaignID == campaignID && x.AppraiseeID == item.AppraiseeID).FirstOrDefault();
                
                if (queryCmteeSequence == null)
                {
                    committeeSequence.Sequence = item.Index;
                    committeeSequence.CampaignID = campaignID;
                    committeeSequence.AppraiseeID = item.AppraiseeID;
                    committeeSequence.IsUpdate = false;
                    list_CommitteeSequence.Add(committeeSequence);
                }
                
            }
            _repoCommitteeSequence.AddRange(list_CommitteeSequence);
            await _repoCommitteeSequence.SaveAll();

            var list_queryCommitteeSequence = await _repoCommitteeSequence.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var data = (from a in model
                        // join b in list_queryCommitteeSequence on new
                        //     {
                        //         a.CampaignID,
                        //         a.AppraiseeID
                        //     } equals new
                        //     {
                        //         b.CampaignID,
                        //         b.AppraiseeID
                        //     }
                        join b in list_queryCommitteeSequence on a.AppraiseeID equals b.AppraiseeID
                        select new PeopleCommitteeDto
                        {
                            AppraiseeID = a.AppraiseeID,
                            L1ID = a.L1ID,
                            Appraisee = a.Appraisee,
                            L1Manager = a.L1Manager,
                            Center = a.Center,
                            Dept = a.Dept,
                            Index = b.Sequence
                        }).OrderBy(x => x.Index).ToList();
            
            return data;
        }

        public async Task<List<HQReportDto>> GetAllHQReport(string lang, int campaignID)
        {
            var index = 1;
            var queryAccountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryEvalution = await _repoEvaluation.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var queryChild = await _repo.FindAll(x => x.L0 == true).ToListAsync();
            var queryParent = await _repo.FindAll().ToListAsync();
            var result = (from a in queryAccountCampaign
                        join c in queryChild on a.AccountID equals c.Id
                        join b in queryEvalution on a.AccountID equals b.UserID
                        let l2Name = _repo.FindById(a.L2) != null ? _repo.FindById(a.L2).FullName : "N/A"
                        let jobTitle = _repoJobTitle.FindById(c.JobTitleId) != null ? lang == "en" ? _repoJobTitle.FindById(c.JobTitleId).NameEn : _repoJobTitle.FindById(c.JobTitleId).NameZh : "N/A"
                        let factory = _repoOc.FindById(c.FactId) != null ? _repoOc.FindById(c.FactId).Name : "N/A"
                        let center = _repoOc.FindById(c.CenterId) != null ? _repoOc.FindById(c.CenterId).Name : "N/A"
                        let dept = _repoOc.FindById(c.DeptId) != null ? _repoOc.FindById(c.DeptId).Name : center
                        
                        select new HQReportDto
                        {
                            ID = c.Id,
                            Factory = factory,
                            Division = "2",
                            Dept = dept,
                            L2Name = l2Name,
                            UserName = c.Username,
                            FullName = c.FullName,
                            JobTitle = jobTitle,
                            // AttitudeScore = CalAttitudeScore(a.AccountID, campaignID),
                            AttitudeScore = CalNewAttitudeScore(a.AccountID, campaignID),
                            KpiScore = getKpiSocreCal(a.AccountID, campaignID, a.L1, a.L2),
                            SpecialScore = _repoCommitteeScore.FindAll(x => x.ScoreTo == c.Id && x.CampaignID == campaignID).FirstOrDefault() != null ? _repoCommitteeScore.FindAll(x => x.ScoreTo == c.Id && x.CampaignID == campaignID).FirstOrDefault().Score.ToDouble() : 0,
                            Index = index
                        }).ToList();
            var model = result.Select(a => new HQReportDto
            {
                ID = a.ID,
                Factory = a.Factory,
                Division = "2",
                Dept = a.Dept,
                L2Name = a.L2Name,
                UserName = a.UserName,
                FullName = a.FullName,
                JobTitle = a.JobTitle,
                AttitudeScore = a.AttitudeScore,
                KpiScore = a.KpiScore,
                SpecialScore = a.SpecialScore,
                H1Score = Math.Round((a.AttitudeScore + a.KpiScore + a.SpecialScore).ToDouble(), 1),
                Index = index
            }).ToList();
            model.ForEach(item => {
                item.Index = index;
                index++;
            });
            
            return model;
        }

        public async Task<string> GetTitleH1HQReport(int campaignID)
        {
            var MonthCampaign = (await _repoCampaign.FindByIdAsync(campaignID)).MonthName;
            string TitleH1 = MonthCampaign == "Jan-June" || MonthCampaign == null ? "H1_HQ_REPORT_LABEL" : "H2_HQ_REPORT_LABEL";
            
            return TitleH1;
        }

        public double getKpiSocreCal(int userID, int campaignID, int l1, int l2) {
            var queryL0 = _repoKPIScore.FindAll(x => x.ScoreTo == userID && x.CampaignID == campaignID && x.ScoreFrom == userID).FirstOrDefault();
            var queryL1 = _repoKPIScore.FindAll(x => x.ScoreTo == userID && x.CampaignID == campaignID && x.ScoreFrom == l1).FirstOrDefault();
            var queryL2 = _repoKPIScore.FindAll(x => x.ScoreTo == userID && x.CampaignID == campaignID && x.ScoreFrom == l2).FirstOrDefault();
            
            double scoreL0 = queryL0 != null ? queryL0.Point.ToDouble() : 0;
            double scoreL1 = queryL1 != null ? queryL1.Point.ToDouble() : 0;
            double scoreL2 = queryL2 != null ? queryL2.Point.ToDouble() : 0;
            
            double scoreTotal = 0;

            if ((scoreL0 != 0 && scoreL1 == 0 && scoreL2 == 0) ||
                (scoreL0 == 0 && scoreL1 != 0 && scoreL2 == 0) ||
                (scoreL0 == 0 && scoreL1 == 0 && scoreL2 != 0))
            {
                scoreTotal = (scoreL0 + scoreL1 + scoreL2)/5*70;
            }
            else if ((scoreL0 != 0 && scoreL1 != 0 && scoreL2 == 0) ||
                     (scoreL0 != 0 && scoreL1 == 0 && scoreL2 != 0) ||
                     (scoreL0 == 0 && scoreL1 != 0 && scoreL2 != 0))
            {
                scoreTotal = ((scoreL0 + scoreL1 + scoreL2)/2)/5*70;
            }
            else if ((scoreL0 != 0 && scoreL1 != 0 && scoreL2 != 0))
            {
                scoreTotal = ((scoreL0 + scoreL1 + scoreL2)/3)/5*70;
            }

            scoreTotal = Math.Round(scoreTotal, 1);

            return scoreTotal;
        }

        public double getAttitudeSocreCal(int scoreTo, int campaignID) {
            double scoreTotal = 0;
            var result = new List<PeopleCommitteeSumScoreAttScoreDto>();

            var score = _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).FirstOrDefault();
            var list_Score = _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToList();
            if (score == null)
            {
                return scoreTotal = 0;
            }

            var queryAccount = _repoAc.FindById(scoreTo);

            var FL = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.FunctionalLeader.HasValue ?  queryAccount.FunctionalLeader.Value : 0,
                        Type = ScoreType.FL,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                    };
            var L0 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = scoreTo,
                        Type = ScoreType.L0,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                    };
            var L1 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.Manager.HasValue ?  queryAccount.Manager.Value : 0,
                        Type = ScoreType.L1,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                    };
            var L2 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.L2.HasValue ?  queryAccount.L2.Value : 0,
                        Type = ScoreType.L2,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                    };

            foreach (var item in list_Score)
            {
                if (item.ScoreBy == queryAccount.FunctionalLeader)
                {
                    FL.ID = queryAccount.FunctionalLeader.HasValue ?  queryAccount.FunctionalLeader.Value : 0;
                    FL.Type = ScoreType.FL;
                    FL.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    FL.SumPoint += item.FLScore.ToDouble();
                }

                if (item.ScoreBy == scoreTo)
                {
                    L0.ID = scoreTo;
                    L0.Type = ScoreType.L0;
                    L0.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L0.SumPoint += item.L0Score.ToDouble();
                }

                if (item.ScoreBy == queryAccount.Manager)
                {
                    L1.ID = queryAccount.Manager.HasValue ?  queryAccount.Manager.Value : 0;
                    L1.Type = ScoreType.L1;
                    L1.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L1.SumPoint += item.L1Score.ToDouble();
                }

                if (item.ScoreBy == queryAccount.L2)
                {
                    L2.ID = queryAccount.L2.HasValue ?  queryAccount.L2.Value : 0;
                    L2.Type = ScoreType.L2;
                    L2.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L2.SumPoint += item.L2Score.ToDouble();
                }
            }

            double scoreFL = 0;
            double scoreL0 = 0;
            double scoreL1 = 0;
            double scoreL2 = 0;

            scoreFL = FL.SumPoint.ToDouble();
            scoreL0 = L0.SumPoint.ToDouble();
            scoreL1 = L1.SumPoint.ToDouble();
            scoreL2 = L2.SumPoint.ToDouble();

            if ((scoreFL != 0 && scoreL1 == 0 && scoreL2 == 0) ||
                (scoreFL == 0 && scoreL1 != 0 && scoreL2 == 0) ||
                (scoreFL == 0 && scoreL1 == 0 && scoreL2 != 0))
            {
                scoreTotal = (scoreFL + scoreL1 + scoreL2)/30*30;
            }
            else if ((scoreFL != 0 && scoreL1 != 0 && scoreL2 == 0) ||
                     (scoreFL != 0 && scoreL1 == 0 && scoreL2 != 0) ||
                     (scoreL0 == 0 && scoreL1 != 0 && scoreL2 != 0))
            {
                scoreTotal = ((scoreFL + scoreL1 + scoreL2)/2)/30*30;
            }
            else if ((scoreFL != 0 && scoreL1 != 0 && scoreL2 != 0))
            {
                scoreTotal = ((scoreFL + scoreL1 + scoreL2)/3)/30*30;
            }

            scoreTotal = Math.Round(scoreTotal, 1);

            return scoreTotal;
        }


        public double CalAttitudeScore(int scoreTo, int campaignID)
        {
            double scoreTotal = 0;
            var result = new List<PeopleCommitteeSumScoreAttScoreDto>();
            var list_Score = _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToList();
            var queryAccount = _repoAc.FindById(scoreTo);
            double scoreFL = list_Score.Where(x => x.ScoreBy == queryAccount.FunctionalLeader).Sum(x => x.FLScore.ToDouble());
            double scoreL1 = list_Score.Where(x => x.ScoreBy == queryAccount.Manager).Sum(x => x.L1Score.ToDouble());
            double scoreL2 = list_Score.Where(x => x.ScoreBy == queryAccount.L2).Sum(x => x.L2Score.ToDouble());

            if (scoreFL > 0 && scoreL1 > 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreFL + scoreL1 + scoreL2) / 3) / 30 * 30;
            }
            if (scoreFL > 0 && scoreL1 > 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreFL + scoreL1) / 2) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 > 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreL1 + scoreL2) / 2) / 30 * 30;
            }

            if (scoreFL > 0 && scoreL1 == 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreFL + scoreL2) / 2) / 30 * 30;
            }

            if (scoreFL > 0 && scoreL1 == 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreFL) / 1) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 == 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreL2) / 1) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 > 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreL1) / 1) / 30 * 30;
            }


            scoreTotal = Math.Round(scoreTotal, 1);

            return scoreTotal;
        }

        public double getH1SocreCal(double kpiScore, double attScore, double specialScore) {
            double scoreTotal = 0;
            scoreTotal = kpiScore + attScore + specialScore;

            scoreTotal = Math.Round(scoreTotal, 1);

            return scoreTotal;
        }

        public double CalNewAttitudeScore(int scoreTo, int campaignID)
        {
            double scoreTotal = 0;
            var accountCampaign = _repoAccountCampaign.FindAll(x => x.AccountID == scoreTo && x.CampaignID == campaignID).FirstOrDefault();
            var list_NewAttScore = _repoNewAttitudeScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo).ToList();
            
            var list_Score = _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToList();

            double scoreFL = list_NewAttScore.Where(x => x.ScoreFrom == accountCampaign.FL)
                                                .Select(x => new PeopleCommitteeSumScoreAttScoreDto
                                                {
                                                    SumPoint = GetPoint(x)
                                                }).ToList().Sum(x => x.SumPoint.ToDouble())/4;
            
            double scoreL1 = list_NewAttScore.Where(x => x.ScoreFrom == accountCampaign.L1)
                                                .Select(x => new PeopleCommitteeSumScoreAttScoreDto
                                                {
                                                    SumPoint = GetPoint(x)
                                                }).ToList().Sum(x => x.SumPoint.ToDouble())/4;

            double scoreL2 = list_NewAttScore.Where(x => x.ScoreFrom == accountCampaign.L2)
                                                .Select(x => new PeopleCommitteeSumScoreAttScoreDto
                                                {
                                                    SumPoint = GetPoint(x)
                                                }).ToList().Sum(x => x.SumPoint.ToDouble())/4;

            if (scoreFL > 0 && scoreL1 > 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreFL + scoreL1 + scoreL2) / 3) / 30 * 30;
            }
            if (scoreFL > 0 && scoreL1 > 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreFL + scoreL1) / 2) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 > 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreL1 + scoreL2) / 2) / 30 * 30;
            }

            if (scoreFL > 0 && scoreL1 == 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreFL + scoreL2) / 2) / 30 * 30;
            }

            if (scoreFL > 0 && scoreL1 == 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreFL) / 1) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 == 0 && scoreL2 > 0)
            {
                scoreTotal = ((scoreL2) / 1) / 30 * 30;
            }

            if (scoreFL == 0 && scoreL1 > 0 && scoreL2 == 0)
            {
                scoreTotal = ((scoreL1) / 1) / 30 * 30;
            }


            scoreTotal = Math.Round(scoreTotal, 1);

            return scoreTotal;
        }
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

        public async Task<PeopleCommitteeDto> GetPeopleCommittee(int appraiseeID)
        {
            var queryChild = await _repo.FindAll(x => x.Id == appraiseeID).ToListAsync();
            var model = from a in queryChild
                        let center = _repoOc.FindById(a.CenterId) != null ? _repoOc.FindById(a.CenterId).Name : "N/A"
                        let dept = _repoOc.FindById(a.DeptId) != null ? _repoOc.FindById(a.DeptId).Name : "N/A"
                        select new PeopleCommitteeDto
                        {
                            AppraiseeID = a.Id,
                            Appraisee = a.FullName,
                            Center = center,
                            Dept = dept
                        };
            return model.FirstOrDefault();
        }

        public async Task<object> GetKpi(int accountID)
        {
            var queryKpiAc = _repoKPIAc.FindAll(x => x.AccountId == accountID);
            var list_KpiAc = (from a in queryKpiAc
                            join b in _repoKPINew.FindAll(x => !x.IsDisplayTodo) on a.KpiId equals b.Id
                            // let nameKpiNew = _repoKPINew.FindById(a.KpiId) != null ? _repoKPINew.FindById(a.KpiId).Name : "N/A"
                            select new 
                            {
                                ID = a.Id,
                                AccountId = a.AccountId,
                                KpiId = a.KpiId,
                                NameKpiNew = b.Name,
                            }).ToList();

            var list_PersonalKpi = new List<object>();
            var list_MultiKpi = new List<object>();

            foreach (var item in list_KpiAc)
            {
                var query = _repoKPIAc.FindAll(x => x.KpiId == item.KpiId).ToList();
                if (query.Count() > 1)
                {
                    // var result = _repoKPIAc.FindAll(x => x.KpiId == KpiAc.KpiId && x.AccountId == accountID).ToList();
                    list_MultiKpi.Add(item);
                }
                else
                {
                    list_PersonalKpi.Add(item);
                }
            }
            return new {
                personal = list_PersonalKpi,
                multi = list_MultiKpi
            };
        }

        public async Task<object> GetAllKpiScore(int scoreTo, int campaignID)
        {
            var result = new List<object>();
            var list_KpiScore = _repoKPIScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToList();
            var hrCommentCmtee = await _repoHRCommentCmtee.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo).FirstOrDefaultAsync();
            foreach (var item in list_KpiScore)
            {
                var data = new List<KPIScoreAttchment>();
                data = await _repoKPIScoreAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreType == item.ScoreType).ToListAsync();

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

                // var query = (from a in list_KpiScore
                //         // let file = _repoKPIScoreAttchment.FindAll(x => x.CampaignID == a.CampaignID && x.ScoreTo == a.ScoreTo && x.ScoreType == a.ScoreType).FirstOrDefault()
                //         // let path = file != null ? file.Path : null
                //         select new {
                //             ID = a.ID,
                //             ScoreType = a.ScoreType,
                //             Point = a.Point,
                //             Comment = a.Comment,
                //             Files = list_file
                //         }).ToList();

                var query = new {
                            ID = item.ID,
                            ScoreType = item.ScoreType,
                            Point = item.Point,
                            Comment = item.Comment,
                            ScoreFromName = _repoAc.FindById(item.ScoreFrom).FullName,
                            HRComment = hrCommentCmtee == null ? null : hrCommentCmtee.Comment,
                            Files = list_file
                        };

                result.Add(query);
            }

            
            
            return result;
            // return new CommitteeKpiScoreDto {
            //     Result = result,
            //     HRComment = hrCommentCmtee == null ? null : hrCommentCmtee.Comment
            // };
        }

        public async Task<object> GetAllAttitudeScore(int scoreTo, int campaignID)
        {
            var result = new List<PeopleCommitteeAttScoreDto>();


            var score = await _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).FirstOrDefaultAsync();
            var list_Score = await _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToListAsync();
            if (score == null)
            {
                return result;
            }

            var queryAccount = _repoAc.FindById(scoreTo);

            // var FL = new PeopleCommitteeAttScoreDto {
            //     ID = score.ID,
            //     Type = ScoreType.FL,
            //     Comment = score.FlComment,
            //     Point = score.FLScore,
            //     Files = getFiles(campaignID, queryAccount.FunctionalLeader.Value, scoreTo)
            // };

            // var L0 = new PeopleCommitteeAttScoreDto {
            //     ID = score.ID,
            //     Type = ScoreType.L0,
            //     Comment = score.L0Comment,
            //     Point = score.L0Score,
            //     Files = getFiles(campaignID, scoreTo, scoreTo)
            // };

            // var L1 = new PeopleCommitteeAttScoreDto {
            //     ID = score.ID,
            //     Type = ScoreType.L1,
            //     Comment = score.L1Comment,
            //     Point = score.L1Score,
            //     Files = getFiles(campaignID, queryAccount.Manager.Value, scoreTo)
            // };

            // var L2 = new PeopleCommitteeAttScoreDto {
            //     ID = score.ID,
            //     Type = ScoreType.L2,
            //     Comment = score.L2Comment,
            //     Point = score.L2Score,
            //     Files = getFiles(campaignID, queryAccount.L2.Value, scoreTo)
            // };
            
            // result.Add(FL);
            // result.Add(L0);
            // result.Add(L1);
            // result.Add(L2);

            foreach (var item in list_Score)
            {
                if (item.ScoreBy == queryAccount.FunctionalLeader)
                {
                    if (item.FlComment != null && item.FlComment != "")
                    {
                        var FL = new PeopleCommitteeAttScoreDto {
                            ID = item.ID,
                            Type = ScoreType.FL,
                            Comment = item.FlComment,
                            Point = item.FLScore,
                            NameAttitudeHeading = _repoAttitudeHeading.FindById(item.AttitudeHeadingID).Name,
                            ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName,
                            Files = getFiles(campaignID, queryAccount.FunctionalLeader.Value, scoreTo, item.AttitudeHeadingID)
                        };

                        result.Add(FL);
                    }
                    
                }

                if (item.ScoreBy == scoreTo)
                {
                    if (item.L0Comment != null && item.L0Comment != "")
                    {
                        var L0 = new PeopleCommitteeAttScoreDto {
                            ID = item.ID,
                            Type = ScoreType.L0,
                            Comment = item.L0Comment,
                            Point = item.L0Score,
                            NameAttitudeHeading = _repoAttitudeHeading.FindById(item.AttitudeHeadingID).Name,
                            ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName,
                            Files = getFiles(campaignID, scoreTo, scoreTo, item.AttitudeHeadingID)
                        };

                        result.Add(L0);
                    }
                    
                }

                if (item.ScoreBy == queryAccount.Manager)
                {
                    if (item.L1Comment != null && item.L1Comment != "")
                    {
                        var L1 = new PeopleCommitteeAttScoreDto {
                            ID = item.ID,
                            Type = ScoreType.L1,
                            Comment = item.L1Comment,
                            Point = item.L1Score,
                            NameAttitudeHeading = _repoAttitudeHeading.FindById(item.AttitudeHeadingID).Name,
                            ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName,
                            Files = getFiles(campaignID, queryAccount.Manager.Value, scoreTo, item.AttitudeHeadingID)
                        };

                        result.Add(L1);
                    }
                    
                }

                if (item.ScoreBy == queryAccount.L2)
                {
                    if (item.L2Comment != null && item.L2Comment != "")
                    {
                        var L2 = new PeopleCommitteeAttScoreDto {
                            ID = item.ID,
                            Type = ScoreType.L2,
                            Comment = item.L2Comment,
                            Point = item.L2Score,
                            NameAttitudeHeading = _repoAttitudeHeading.FindById(item.AttitudeHeadingID).Name,
                            ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName,
                            Files = getFiles(campaignID, queryAccount.L2.Value, scoreTo, item.AttitudeHeadingID)
                        };

                        result.Add(L2);
                    }
                    
                }
            }
            
            return result;
        }

        public object getFiles(int campaignID, int uploadFrom, int uploadTo, int attitudeHeadingID) {
            var data = new List<AttitudeAttchment>();
            data = _repoAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.UploadFrom == uploadFrom && x.UploadTo == uploadTo && x.HeadingID == attitudeHeadingID).ToList();

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

            return list_file;
        }

        public object getFilesAttitudeScore(int campaignID, int uploadFrom, int uploadTo) {
            var data = new List<AttitudeAttchment>();
            data = _repoAttitudeAttchment.FindAll(x => x.CampaignID == campaignID && x.UploadFrom == uploadFrom && x.UploadTo == uploadTo).ToList();

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

            return list_file;
        }

        public async Task<object> GetSumAttitudeScore(int scoreTo, int campaignID)
        {
            var result = new List<PeopleCommitteeSumScoreAttScoreDto>();


            var score = await _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).FirstOrDefaultAsync();
            var list_Score = await _repoScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToListAsync();
            if (score == null)
            {
                return result;
            }

            var queryAccount = _repoAc.FindById(scoreTo);

            var FL = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.FunctionalLeader.HasValue ?  queryAccount.FunctionalLeader.Value : 0,
                        Type = ScoreType.FL,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                        Files = getFilesAttitudeScore(campaignID, queryAccount.FunctionalLeader.Value, scoreTo),
                    };
            var L0 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = scoreTo,
                        Type = ScoreType.L0,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                        Files = getFilesAttitudeScore(campaignID, scoreTo, scoreTo),
                    };
            var L1 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.Manager.HasValue ?  queryAccount.Manager.Value : 0,
                        Type = ScoreType.L1,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                        Files = getFilesAttitudeScore(campaignID, queryAccount.Manager.Value, scoreTo),
                    };
            var L2 = new PeopleCommitteeSumScoreAttScoreDto {
                        ID = queryAccount.L2.HasValue ?  queryAccount.L2.Value : 0,
                        Type = ScoreType.L2,
                        ScoreFromName = "N/A",
                        SumPoint = 0,
                        Files = getFilesAttitudeScore(campaignID, queryAccount.L2.Value, scoreTo),
                    };

            foreach (var item in list_Score)
            {
                if (item.ScoreBy == queryAccount.FunctionalLeader)
                {
                    FL.ID = queryAccount.FunctionalLeader.HasValue ?  queryAccount.FunctionalLeader.Value : 0;
                    FL.Type = ScoreType.FL;
                    FL.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    FL.SumPoint += item.FLScore.ToDouble();
                    FL.Files = getFilesAttitudeScore(campaignID, queryAccount.FunctionalLeader.Value, scoreTo);
                }

                if (item.ScoreBy == scoreTo)
                {
                    L0.ID = scoreTo;
                    L0.Type = ScoreType.L0;
                    L0.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L0.SumPoint += item.L0Score.ToDouble();
                    L0.Files = getFilesAttitudeScore(campaignID, scoreTo, scoreTo);
                }

                if (item.ScoreBy == queryAccount.Manager)
                {
                    L1.ID = queryAccount.Manager.HasValue ?  queryAccount.Manager.Value : 0;
                    L1.Type = ScoreType.L1;
                    L1.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L1.SumPoint += item.L1Score.ToDouble();
                    L1.Files = getFilesAttitudeScore(campaignID, queryAccount.Manager.Value, scoreTo);
                }

                if (item.ScoreBy == queryAccount.L2)
                {
                    L2.ID = queryAccount.L2.HasValue ?  queryAccount.L2.Value : 0;
                    L2.Type = ScoreType.L2;
                    L2.ScoreFromName = _repoAc.FindById(item.ScoreBy).FullName;
                    L2.SumPoint += item.L2Score.ToDouble();
                    L2.Files = getFilesAttitudeScore(campaignID, queryAccount.L2.Value, scoreTo);
                }
            }

            result.Add(FL);
            result.Add(L0);
            result.Add(L1);
            result.Add(L2);
            
            return result;
        }

        public async Task<object> GetSpecialScoreDetail(int scoreTo, int campaignID)
        {
            var queryAccount = await _repoAc.FindByIdAsync(scoreTo);
            
            var query = await _repoSpecialContributionScore.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreFrom == queryAccount.Manager
                && x.ScoreTo == scoreTo
                && x.ScoreType == ScoreType.L1
            ).FirstOrDefaultAsync();
            if (query == null)
            {
                // return new PeopleCommitteeSpecialScoreDto{};
                return query;
            }
             var result = new PeopleCommitteeSpecialScoreDto {
                 ID = query.ID,
                 Content = query.Content,
                 Subject = query.Subject,
                 TypeID = query.TypeID,
                 CompactID = query.CompactID,
                 Ratio = query.Ratio,
                 Point = query.Point,
                 Files = getFilesSpecialScore(campaignID, scoreTo, ScoreType.L1)
             };
            return result;
        }

        public object getFilesSpecialScore(int campaignID, int scoreTo, string scoreType) {
            var data = new List<KPIScoreAttchment>();
            data = _repoKPIScoreAttchment.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreType == scoreType).ToList();

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

            return list_file;
        }

        public async Task<List<PeopleCommitteeSpecialScoreDto>> GetScoreL2(int scoreTo, int campaignID)
        {
            var queryAccount = await _repoAc.FindByIdAsync(scoreTo);
            
            var query = await _repoSpecialContributionScore.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreFrom == queryAccount.L2
                && x.ScoreTo == scoreTo
                && x.ScoreType == ScoreType.L2
            ).ToListAsync();
            if (query == null)
            {
                return new List<PeopleCommitteeSpecialScoreDto>();
            }

            var model = (from a in query
                        select new PeopleCommitteeSpecialScoreDto
                        {
                            ID = a.ID,
                            Content = a.Content,
                            Point = a.Point,
                            Files = getFilesSpecialScore(campaignID, scoreTo, ScoreType.L2)
                        }).ToList();
             
            return model;
        }

        public async Task<CommitteeScoreDto> GetCommitteeScore(int scoreTo, int scoreFrom, int campaignID)
        {
            var query = await _repoCommitteeScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreFrom).ProjectTo<CommitteeScoreDto>(_configMapper).FirstOrDefaultAsync();
            return query;
        }

        public async Task<bool> GetFrozen(int campaignID)
        {
            var query = await _repoCommitteeSequence.FindAll(x => x.CampaignID == campaignID).FirstOrDefaultAsync();
            if (query == null)
            {
                return false;
            }

            return query.IsUpdate ?? false;
        }

        public async Task<OperationResult> UpdateKpiScore(KPIScoreDto model)
        {
            var item = await _repoKPIScore.FindByIdAsync(model.ID);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = MessageReponse.UpdateError, Success = false };
            }
            item.Comment = model.Comment;
            item.Point = model.Point;
            var hrCommentCmtee = await _repoHRCommentCmtee.FindAll(x => x.CampaignID == item.CampaignID && x.ScoreTo == item.ScoreTo).FirstOrDefaultAsync();
            if (hrCommentCmtee == null)
            {
                var item_hrCommentCmtee = new HRCommentCmtee();
                item_hrCommentCmtee.CampaignID = item.CampaignID;
                item_hrCommentCmtee.ScoreTo = item.ScoreTo;
                item_hrCommentCmtee.CreatedBy = model.HRCommentCreatedBy;
                item_hrCommentCmtee.CreatedTime = DateTime.Now;
                item_hrCommentCmtee.Comment = model.HRComment;
                _repoHRCommentCmtee.Add(item_hrCommentCmtee);
            }
            else
            {
                hrCommentCmtee.UpdateBy = model.HRCommentUpdateBy;
                hrCommentCmtee.Comment = model.HRComment;
                _repoHRCommentCmtee.Update(hrCommentCmtee);
            }
            try
            {
                _repoKPIScore.Update(item);
                await _repoKPIScore.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateAttitudeScore(PeopleCommitteeAttScoreDto model)
        {
            var item = await _repoScore.FindByIdAsync(model.ID);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = MessageReponse.UpdateError, Success = false };
            }
            switch (model.Type)
                {
                    case "FL":
                        item.FlComment = model.Comment;
                        item.FLScore = model.Point;
                        break;
                    case "L0":
                        item.L0Comment = model.Comment;
                        item.L0Score = model.Point;
                        break;
                    case "L1":
                        item.L1Comment = model.Comment;
                        item.L1Score = model.Point;
                        break;
                    case "L2":
                        item.L2Comment = model.Comment;
                        item.L2Score = model.Point;
                        break;
                }
            try
            {
                _repoScore.Update(item);
                await _repoScore.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateSpecialScore(SpecialContributionScoreDto model)
        {
            var item = await _repoSpecialContributionScore.FindByIdAsync(model.ID);
            var L1OfUser = _repoAc.FindById(model.ScoreTo).Manager;
            
            if (item == null)
            {
                var add = _mapper.Map<SpecialContributionScore>(model);
                add.CreatedTime = DateTime.Now;
                add.ScoreBy = L1OfUser.Value;
                add.ScoreFrom = L1OfUser.Value;
                add.IsSubmit = true;
                _repoSpecialContributionScore.Add(add);
            }
            else
            {
                item.TypeID = model.TypeID;
                item.CompactID = model.CompactID;
                item.Content = model.Content;
                item.Subject = model.Subject;
                item.Ratio = model.Ratio;
                item.Point = model.Point;
                _repoSpecialContributionScore.Update(item);
            }
            try
            {
                
                await _repoSpecialContributionScore.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateKpiScoreL2(PeopleCommitteeSpecialScoreDto model)
        {
            var item = await _repoSpecialContributionScore.FindByIdAsync(model.ID);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = MessageReponse.UpdateError, Success = false };
            }
            item.Content = model.Content;
            item.Point = model.Point;
            try
            {
                _repoSpecialContributionScore.Update(item);
                await _repoSpecialContributionScore.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateCommitteeScore(CommitteeScoreDto model)
        {
            var query = await _repoCommitteeScore.FindAll(x => x.CampaignID == model.CampaignID && x.ScoreTo == model.ScoreTo && x.ScoreFrom == model.ScoreFrom).FirstOrDefaultAsync();
            if (query == null)
            {
                var add = _mapper.Map<CommitteeScore>(model);
                _repoCommitteeScore.Add(add);
            }
            else
            {
                query.Score = model.Score;
                _repoCommitteeScore.Update(query);
            }
            try
            {
                await _repoCommitteeScore.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = query
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateCommitteeSequence(CommitteeSequenceDto model)
        {
            var query = await _repoCommitteeSequence.FindAll(x => x.CampaignID == model.CampaignID && x.AppraiseeID == model.AppraiseeID).FirstOrDefaultAsync();
            query.Sequence = model.ToIndex;
            _repoCommitteeSequence.Update(query);

            var list_CommitteeSequence = await _repoCommitteeSequence.FindAll(x => x.CampaignID == model.CampaignID && x.AppraiseeID != model.AppraiseeID).ToListAsync();
            foreach (var item in list_CommitteeSequence)
            {
                if (item.Sequence >= model.ToIndex && item.Sequence <= model.FromIndex)
                {
                    item.Sequence = item.Sequence + 1;
                }
                if (item.Sequence <= model.ToIndex && item.Sequence >= model.FromIndex)
                {
                    item.Sequence = item.Sequence - 1;
                }
                
            }
            _repoCommitteeSequence.UpdateRange(list_CommitteeSequence);
            try
            {
                await _repoCommitteeSequence.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = query
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> LockUpdate(int campaignID)
        {
            var query = await _repoCommitteeSequence.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            if (query.Count() == 0)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "ERROR_LOCK_UPDATE_CMTEE_MESSAGE", Success = false };
            }
            query.ForEach(x => x.IsUpdate = !x.IsUpdate);
            _repoCommitteeSequence.UpdateRange(query);
            try
            {
                await _repoCommitteeSequence.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = _repoCommitteeSequence.FindAll(x => x.CampaignID == campaignID).FirstOrDefault().IsUpdate == true ? "LOCK_UPDATE_CMTEE_MESSAGE" : "UNLOCK_UPDATE_CMTEE_MESSAGE",
                    Success = true,
                    Data = query
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
