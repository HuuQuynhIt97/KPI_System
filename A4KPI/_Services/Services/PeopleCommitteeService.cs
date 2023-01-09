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

    public class PeopleCommitteeService : IPeopleCommitteeService
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
        private readonly IPerfomanceEvaluationTypeRepository _repoPerfomanceType;
        private readonly IPerfomanceEvaluationImpactRepository _repoPerfomanceImpact;
        private readonly ICommitteeScoreRepository _repoCommitteeScore;
        private readonly IAttitudeHeadingRepository _repoAttitudeHeading;
        private readonly IHRCommentCmteeRepository _repoHRCommentCmtee;
        private readonly ICommitteeSequenceRepository _repoCommitteeSequence;
        private readonly IAttitudeSubmitRepository _repoAttitudeSubmit;
        private readonly INewAttitudeScoreRepository _repoNewAttitudeScore;
        private readonly INewAttitudeEvaluationRepository _repoNewAttitudeEvaluation;
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
        public PeopleCommitteeService(
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
            IPerfomanceEvaluationTypeRepository repoPerfomanceType,
            IPerfomanceEvaluationImpactRepository repoPerfomanceImpact,
            ICommitteeScoreRepository repoCommitteeScore,
            IAttitudeHeadingRepository repoAttitudeHeading,
            IHRCommentCmteeRepository repoHRCommentCmtee,
            ICommitteeSequenceRepository repoCommitteeSequence,
            IAttitudeSubmitRepository repoAttitudeSubmit,
            INewAttitudeScoreRepository repoNewAttitudeScore,
            INewAttitudeEvaluationRepository repoNewAttitudeEvaluation,
            IAccountCampaignRepository repoAccountCampaign,
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
            _repoPerfomanceType = repoPerfomanceType;
            _repoPerfomanceImpact = repoPerfomanceImpact;
            _repoCommitteeScore = repoCommitteeScore;
            _repoAttitudeHeading = repoAttitudeHeading;
            _repoHRCommentCmtee = repoHRCommentCmtee;
            _repoCommitteeSequence = repoCommitteeSequence;
            _repoAttitudeSubmit = repoAttitudeSubmit;
            _repoNewAttitudeScore = repoNewAttitudeScore;
            _repoNewAttitudeEvaluation = repoNewAttitudeEvaluation;
            _repoAccountCampaign = repoAccountCampaign;
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

        private object GetListYTD(int kpiID, int campaignID)
        {
            List<double> listYTD = new List<double>();
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
                    double dataTarget = data.FirstOrDefault(x => x.TargetTime.Month == item && x.TargetTime.Year == yearNum).YTD;
                    listYTD.Add(dataTarget);
                }
                else
                {
                    listYTD.Add(0);
                }
            }
            return listYTD;
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
                              ytds = GetListYTD(y.Id, campaignID),
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
                              ytds = GetListYTD(y.Id, campaignID),
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
            var accountCampaign = await _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID).ToListAsync();
            var model = (from a in accountCampaign
                        join b in queryEvalution on a.AccountID equals b.UserID 
                        join c in queryChild on a.AccountID equals c.Id
                        let manager = _repo.FindById(a.L1) != null ? _repo.FindById(a.L1).FullName : "N/A"
                        let center = _repoOc.FindById(c.CenterId) != null ? _repoOc.FindById(c.CenterId).Name : "N/A"
                        let dept = _repoOc.FindById(c.DeptId) != null ? _repoOc.FindById(c.DeptId).Name : "N/A"
                        select new PeopleCommitteeDto
                        {
                            AppraiseeID = a.AccountID,
                            // L1ID = a.Manager.Value,
                            L1ID = a.L1,
                            // Appraisee = a.FullName,
                            Appraisee = c.FullName,
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

        public async Task<object> GetSumNewAttitudeScore(int scoreTo, int campaignID)
        {
            var result = new List<PeopleCommitteeSumScoreAttScoreDto>();


            var score = await _repoNewAttitudeScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).FirstOrDefaultAsync();
            var attitudeSubmit = await _repoAttitudeSubmit.FindAll(x => x.SubmitTo == scoreTo && x.CampaignID == campaignID).FirstOrDefaultAsync();
            var list_Score = await _repoNewAttitudeScore.FindAll(x => x.ScoreTo == scoreTo && x.CampaignID == campaignID).ToListAsync();
            if (score == null)
            {
                return result;
            }

            var queryAccount = _repoAc.FindById(scoreTo);
            var queryAccountCampaign = _repoAccountCampaign.FindAll(x => x.AccountID == scoreTo && x.CampaignID == campaignID).FirstOrDefault();

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
                if (item.ScoreFrom == queryAccountCampaign.FL && attitudeSubmit.IsSubmitAttitudeFL)
                {
                    FL.ID = queryAccountCampaign.FL;
                    FL.Type = ScoreType.FL;
                    FL.ScoreFromName = _repoAc.FindById(item.ScoreFrom).FullName;
                    FL.SumPoint += GetPoint(item).ToDouble();
                }

                if (item.ScoreFrom == scoreTo && attitudeSubmit.IsSubmitAttitudeL0)
                {
                    L0.ID = scoreTo;
                    L0.Type = ScoreType.L0;
                    L0.ScoreFromName = _repoAc.FindById(item.ScoreFrom).FullName;
                    L0.SumPoint += GetPoint(item).ToDouble();
                }

                if (item.ScoreFrom == queryAccountCampaign.L1 && attitudeSubmit.IsSubmitAttitudeL1)
                {
                    L1.ID = queryAccountCampaign.L1;
                    L1.Type = ScoreType.L1;
                    L1.ScoreFromName = _repoAc.FindById(item.ScoreFrom).FullName;
                    L1.SumPoint += GetPoint(item).ToDouble();
                }

                if (item.ScoreFrom == queryAccountCampaign.L2 && attitudeSubmit.IsSubmitAttitudeL2)
                {
                    L2.ID = queryAccountCampaign.L2;
                    L2.Type = ScoreType.L2;
                    L2.ScoreFromName = _repoAc.FindById(item.ScoreFrom).FullName;
                    L2.SumPoint += GetPoint(item).ToDouble();
                }
            }

            FL.SumPoint /= 4;
            result.Add(FL);
            L0.SumPoint /= 4;
            result.Add(L0);
            L1.SumPoint /= 4;
            result.Add(L1);
            L2.SumPoint /= 4;
            result.Add(L2);
            
            return result;
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

        public async Task<NewAttitudeEvaluationDetailDto> GetDetailNewAttitudeEvaluation(int scoreTo, int campaignID)
        {
            var account = _repoAc.FindById(scoreTo);
            var accountCampaign = _repoAccountCampaign.FindAll(x => x.CampaignID == campaignID && x.AccountID == scoreTo).FirstOrDefault();
            if (account == null)
            {
                return new NewAttitudeEvaluationDetailDto();
            }

            var newAttitudeEvaluationL0 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == scoreTo).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL0).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationL1 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.L1).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL1).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationL2 = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.L2).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeL2).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluationFL = (from a in await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo && x.ScoreFrom == accountCampaign.FL).ToListAsync()
                                            join b in await _repoAttitudeSubmit.FindAll(x => x.CampaignID == campaignID && x.IsSubmitAttitudeFL).ToListAsync() on a.ScoreTo equals b.SubmitTo
                                            select a).FirstOrDefault();

            var newAttitudeEvaluation = new NewAttitudeEvaluationDetailDto();
            newAttitudeEvaluation.CampaignID = campaignID;
            // L0
            newAttitudeEvaluation.FirstQuestion1L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion1 : false;
            newAttitudeEvaluation.FirstQuestion2L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion2 : false;
            newAttitudeEvaluation.FirstQuestion3L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion3 : false;
            newAttitudeEvaluation.FirstQuestion4L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion4 : false;
            newAttitudeEvaluation.FirstQuestion5L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion5 : false;
            newAttitudeEvaluation.FirstQuestion6L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FirstQuestion6 : false;

            newAttitudeEvaluation.SecondQuestion1L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion1 : false;
            newAttitudeEvaluation.SecondQuestion2L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion2 : false;
            newAttitudeEvaluation.SecondQuestion3L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion3 : false;
            newAttitudeEvaluation.SecondQuestion4L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion4 : false;
            newAttitudeEvaluation.SecondQuestion5L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion5 : false;
            newAttitudeEvaluation.SecondQuestion6L0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.SecondQuestion6 : false;

            newAttitudeEvaluation.ThirdQuestionL0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.ThirdQuestion : "";
            newAttitudeEvaluation.FourthQuestionL0 = newAttitudeEvaluationL0 != null ? newAttitudeEvaluationL0.FourthQuestion : "";

            newAttitudeEvaluation.L0ID = accountCampaign.AccountID;

            // L1
            newAttitudeEvaluation.FirstQuestion1L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion1 : false;
            newAttitudeEvaluation.FirstQuestion2L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion2 : false;
            newAttitudeEvaluation.FirstQuestion3L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion3 : false;
            newAttitudeEvaluation.FirstQuestion4L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion4 : false;
            newAttitudeEvaluation.FirstQuestion5L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion5 : false;
            newAttitudeEvaluation.FirstQuestion6L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.FirstQuestion6 : false;

            newAttitudeEvaluation.SecondQuestion1L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion1 : false;
            newAttitudeEvaluation.SecondQuestion2L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion2 : false;
            newAttitudeEvaluation.SecondQuestion3L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion3 : false;
            newAttitudeEvaluation.SecondQuestion4L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion4 : false;
            newAttitudeEvaluation.SecondQuestion5L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion5 : false;
            newAttitudeEvaluation.SecondQuestion6L1 = newAttitudeEvaluationL1 != null ? newAttitudeEvaluationL1.SecondQuestion6 : false;

            newAttitudeEvaluation.ThirdQuestionL1 = newAttitudeEvaluationL1 != null ?  newAttitudeEvaluationL1.ThirdQuestion : "";
            
            newAttitudeEvaluation.L1ID = accountCampaign.L1;
            
            // L2, FL
            newAttitudeEvaluation.CommentL2 = newAttitudeEvaluationL2 != null ? newAttitudeEvaluationL2.ThirdQuestion : "";
            newAttitudeEvaluation.L2ID = accountCampaign.L2;
            newAttitudeEvaluation.CommentFL = newAttitudeEvaluationFL != null ? newAttitudeEvaluationFL.ThirdQuestion : "";
            newAttitudeEvaluation.FLID = accountCampaign.FL;

            return newAttitudeEvaluation;
            
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
            var query = await _repoCommitteeScore.FindAll(x => x.CampaignID == campaignID && x.ScoreTo == scoreTo).ProjectTo<CommitteeScoreDto>(_configMapper).FirstOrDefaultAsync();
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

        public async Task<bool> UpdateSpecialContribution(SpecialContributionScoreDto model)
        {
            var accountCampaign = _repoAccountCampaign.FindAll(x => x.CampaignID == model.CampaignID && x.AccountID == model.ScoreTo).FirstOrDefault();
            var item = _repoSpecialContributionScore.FindAll(
                x => x.CampaignID == model.CampaignID
                && x.ScoreBy == accountCampaign.L1
                && x.ScoreTo == model.ScoreTo
                && x.ScoreType == model.ScoreType
                ).FirstOrDefault();
            if (item == null)
            {
                var add = _mapper.Map<SpecialContributionScore>(model);
                add.CreatedTime = DateTime.Now;
                add.ScoreBy = accountCampaign.L1;
                add.ScoreFrom = accountCampaign.L1;
                _repoSpecialContributionScore.Add(add);

                // add type
                var type = new List<PerfomanceEvaluationType>();
                if (model.TypeListID.Count > 0)
                {
                    foreach (var item_type in model.TypeListID)
                    {
                        var item_add = new PerfomanceEvaluationType
                        {
                            CampaignID = model.CampaignID,
                            ScoreFrom = accountCampaign.L1,
                            ScoreTo = model.ScoreTo,
                            TypeID = item_type,
                            Type = model.ScoreType,
                            CreatedTime = DateTime.Now
                        };
                        type.Add(item_add);
                    };

                }

                _repoPerfomanceType.AddRange(type);
                // add impact
                var impact = new List<PerfomanceEvaluationImpact>();
                if (model.CompactListID.Count > 0)
                {
                    foreach (var item_impact in model.CompactListID)
                    {
                        var item_add = new PerfomanceEvaluationImpact
                        {
                            CampaignID = model.CampaignID,
                            ScoreFrom = accountCampaign.L1,
                            ScoreTo = model.ScoreTo,
                            ImpactID = item_impact,
                            Type = model.ScoreType,
                            CreatedTime = DateTime.Now
                        };
                        impact.Add(item_add);

                    };

                }

                _repoPerfomanceImpact.AddRange(impact);
            }
            else
            {
                // xoa type
                var item_del_type = _repoPerfomanceType.FindAll(x => x.CampaignID == model.CampaignID
                && x.ScoreFrom == accountCampaign.L1
                && x.ScoreTo == model.ScoreTo
                && x.Type == model.ScoreType).ToList();

                if (item_del_type.Count > 0)
                {
                    _repoPerfomanceType.RemoveMultiple(item_del_type);
                    await _repoPerfomanceType.SaveAll();
                }
                // xoa impact

                var item_del_impact = _repoPerfomanceImpact.FindAll(x => x.CampaignID == model.CampaignID
                && x.ScoreFrom == accountCampaign.L1
                && x.ScoreTo == model.ScoreTo
                && x.Type == model.ScoreType).ToList();

                if (item_del_impact.Count > 0)
                {
                    _repoPerfomanceImpact.RemoveMultiple(item_del_impact);
                    await _repoPerfomanceImpact.SaveAll();
                }

                // add lai

                // add type
                var type = new List<PerfomanceEvaluationType>();
                if (model.TypeListID.Count > 0)
                {
                    foreach (var item_type in model.TypeListID)
                    {
                        var item_add = new PerfomanceEvaluationType
                        {
                            CampaignID = model.CampaignID,
                            ScoreFrom = accountCampaign.L1,
                            ScoreTo = model.ScoreTo,
                            TypeID = item_type,
                            Type = model.ScoreType,
                            CreatedTime = DateTime.Now
                        };
                        type.Add(item_add);
                    };

                }

                _repoPerfomanceType.AddRange(type);
                // add impact
                var impact = new List<PerfomanceEvaluationImpact>();
                if (model.CompactListID.Count > 0)
                {
                    foreach (var item_impact in model.CompactListID)
                    {
                        var item_add = new PerfomanceEvaluationImpact
                        {
                            CampaignID = model.CampaignID,
                            ScoreFrom = accountCampaign.L1,
                            ScoreTo = model.ScoreTo,
                            ImpactID = item_impact,
                            Type = model.ScoreType,
                            CreatedTime = DateTime.Now
                        };
                        impact.Add(item_add);

                    };

                }
                _repoPerfomanceImpact.AddRange(impact);

                // update special score
                item.IsSubmit = model.IsSubmit;
                item.Point = model.Point;
                item.Content = model.Content;
                item.Subject = model.Subject;
                item.TypeID = model.TypeID;
                item.CompactID = model.CompactID;
                item.Ratio = model.Ratio;
                _repoSpecialContributionScore.Update(item);
            }

            try
            {
                await _repo.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
            var query = await _repoCommitteeScore.FindAll(x => x.CampaignID == model.CampaignID && x.ScoreTo == model.ScoreTo).FirstOrDefaultAsync();
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

        public async Task<OperationResult> UpdateNewAttitudeEvaluation(NewAttitudeEvaluationDetailDto model)
        {   
            var checkSubmited = _repoAttitudeSubmit.FindAll(x => x.CampaignID == model.CampaignID && x.SubmitTo == model.L0ID).FirstOrDefault();
            
            //update FL
            if (model.FLID != 0 && checkSubmited.IsSubmitAttitudeFL)
            {
                var item = await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == model.CampaignID && x.Type == ScoreType.FL && x.ScoreTo == model.L0ID && x.ScoreFrom == model.FLID).FirstOrDefaultAsync();
                if (item == null)
                {
                    var add_NewAttitudeEvaluation = new NewAttitudeEvaluation();
                    add_NewAttitudeEvaluation.CreatedTime = DateTime.Now;
                    add_NewAttitudeEvaluation.CampaignID = model.CampaignID;
                    add_NewAttitudeEvaluation.ScoreTo = model.L0ID;
                    add_NewAttitudeEvaluation.ScoreFrom = model.FLID;
                    add_NewAttitudeEvaluation.Type = ScoreType.FL;

                    add_NewAttitudeEvaluation.ThirdQuestion = model.CommentFL;
                    _repoNewAttitudeEvaluation.Add(add_NewAttitudeEvaluation);
                }
                else
                {
                    item.ThirdQuestion = model.CommentFL;
                    _repoNewAttitudeEvaluation.Update(item);
                }
            }

            //update L2
            if (model.L2ID != 0 && checkSubmited.IsSubmitAttitudeL2)
            {
                var item = await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == model.CampaignID && x.Type == ScoreType.L2 && x.ScoreTo == model.L0ID && x.ScoreFrom == model.L2ID).FirstOrDefaultAsync();
                if (item == null)
                {
                    var add_NewAttitudeEvaluation = new NewAttitudeEvaluation();
                    add_NewAttitudeEvaluation.CreatedTime = DateTime.Now;
                    add_NewAttitudeEvaluation.CampaignID = model.CampaignID;
                    add_NewAttitudeEvaluation.ScoreTo = model.L0ID;
                    add_NewAttitudeEvaluation.ScoreFrom = model.L2ID;
                    add_NewAttitudeEvaluation.Type = ScoreType.L2;

                    add_NewAttitudeEvaluation.ThirdQuestion = model.CommentL2;
                    _repoNewAttitudeEvaluation.Add(add_NewAttitudeEvaluation);
                }
                else
                {
                    item.ThirdQuestion = model.CommentL2;
                    _repoNewAttitudeEvaluation.Update(item);
                }
            }

            //update L1
            if (model.L1ID != 0 && checkSubmited.IsSubmitAttitudeL1)
            {
                var item = await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == model.CampaignID && x.Type == ScoreType.L1 && x.ScoreTo == model.L0ID && x.ScoreFrom == model.L1ID).FirstOrDefaultAsync();
                if (item == null)
                {
                    var add_NewAttitudeEvaluation = new NewAttitudeEvaluation();
                    add_NewAttitudeEvaluation.CreatedTime = DateTime.Now;
                    add_NewAttitudeEvaluation.CampaignID = model.CampaignID;
                    add_NewAttitudeEvaluation.ScoreTo = model.L0ID;
                    add_NewAttitudeEvaluation.ScoreFrom = model.L1ID;
                    add_NewAttitudeEvaluation.Type = ScoreType.L1;

                    add_NewAttitudeEvaluation.FirstQuestion1 = model.FirstQuestion1L1;
                    add_NewAttitudeEvaluation.FirstQuestion2 = model.FirstQuestion2L1;
                    add_NewAttitudeEvaluation.FirstQuestion3 = model.FirstQuestion3L1;
                    add_NewAttitudeEvaluation.FirstQuestion4 = model.FirstQuestion4L1;
                    add_NewAttitudeEvaluation.FirstQuestion5 = model.FirstQuestion5L1;
                    add_NewAttitudeEvaluation.FirstQuestion6 = model.FirstQuestion6L1;

                    add_NewAttitudeEvaluation.SecondQuestion1 = model.SecondQuestion1L1;
                    add_NewAttitudeEvaluation.SecondQuestion2 = model.SecondQuestion2L1;
                    add_NewAttitudeEvaluation.SecondQuestion3 = model.SecondQuestion3L1;
                    add_NewAttitudeEvaluation.SecondQuestion4 = model.SecondQuestion4L1;
                    add_NewAttitudeEvaluation.SecondQuestion5 = model.SecondQuestion5L1;
                    add_NewAttitudeEvaluation.SecondQuestion6 = model.SecondQuestion6L1;
                    
                    add_NewAttitudeEvaluation.ThirdQuestion = model.ThirdQuestionL1;
                    _repoNewAttitudeEvaluation.Add(add_NewAttitudeEvaluation);
                }
                else
                {
                    item.FirstQuestion1 = model.FirstQuestion1L1;
                    item.FirstQuestion2 = model.FirstQuestion2L1;
                    item.FirstQuestion3 = model.FirstQuestion3L1;
                    item.FirstQuestion4 = model.FirstQuestion4L1;
                    item.FirstQuestion5 = model.FirstQuestion5L1;
                    item.FirstQuestion6 = model.FirstQuestion6L1;

                    item.SecondQuestion1 = model.SecondQuestion1L1;
                    item.SecondQuestion2 = model.SecondQuestion2L1;
                    item.SecondQuestion3 = model.SecondQuestion3L1;
                    item.SecondQuestion4 = model.SecondQuestion4L1;
                    item.SecondQuestion5 = model.SecondQuestion5L1;
                    item.SecondQuestion6 = model.SecondQuestion6L1;
                    
                    item.ThirdQuestion = model.ThirdQuestionL1;
                    _repoNewAttitudeEvaluation.Update(item);
                }
            }

            //update L0
            if (model.L0ID != 0 && checkSubmited.IsSubmitAttitudeL0)
            {
                var item = await _repoNewAttitudeEvaluation.FindAll(x => x.CampaignID == model.CampaignID && x.Type == ScoreType.L0 && x.ScoreTo == model.L0ID && x.ScoreFrom == model.L0ID).FirstOrDefaultAsync();
                if (item == null)
                {
                    var add_NewAttitudeEvaluation = new NewAttitudeEvaluation();
                    add_NewAttitudeEvaluation.CreatedTime = DateTime.Now;
                    add_NewAttitudeEvaluation.CampaignID = model.CampaignID;
                    add_NewAttitudeEvaluation.ScoreTo = model.L0ID;
                    add_NewAttitudeEvaluation.ScoreFrom = model.L0ID;
                    add_NewAttitudeEvaluation.Type = ScoreType.L0;

                    add_NewAttitudeEvaluation.FirstQuestion1 = model.FirstQuestion1L0;
                    add_NewAttitudeEvaluation.FirstQuestion2 = model.FirstQuestion2L0;
                    add_NewAttitudeEvaluation.FirstQuestion3 = model.FirstQuestion3L0;
                    add_NewAttitudeEvaluation.FirstQuestion4 = model.FirstQuestion4L0;
                    add_NewAttitudeEvaluation.FirstQuestion5 = model.FirstQuestion5L0;
                    add_NewAttitudeEvaluation.FirstQuestion6 = model.FirstQuestion6L0;

                    add_NewAttitudeEvaluation.SecondQuestion1 = model.SecondQuestion1L0;
                    add_NewAttitudeEvaluation.SecondQuestion2 = model.SecondQuestion2L0;
                    add_NewAttitudeEvaluation.SecondQuestion3 = model.SecondQuestion3L0;
                    add_NewAttitudeEvaluation.SecondQuestion4 = model.SecondQuestion4L0;
                    add_NewAttitudeEvaluation.SecondQuestion5 = model.SecondQuestion5L0;
                    add_NewAttitudeEvaluation.SecondQuestion6 = model.SecondQuestion6L0;
                    
                    add_NewAttitudeEvaluation.ThirdQuestion = model.ThirdQuestionL0;
                    add_NewAttitudeEvaluation.FourthQuestion = model.FourthQuestionL0;
                    _repoNewAttitudeEvaluation.Add(add_NewAttitudeEvaluation);
                }
                else
                {
                    item.FirstQuestion1 = model.FirstQuestion1L0;
                    item.FirstQuestion2 = model.FirstQuestion2L0;
                    item.FirstQuestion3 = model.FirstQuestion3L0;
                    item.FirstQuestion4 = model.FirstQuestion4L0;
                    item.FirstQuestion5 = model.FirstQuestion5L0;
                    item.FirstQuestion6 = model.FirstQuestion6L0;

                    item.SecondQuestion1 = model.SecondQuestion1L0;
                    item.SecondQuestion2 = model.SecondQuestion2L0;
                    item.SecondQuestion3 = model.SecondQuestion3L0;
                    item.SecondQuestion4 = model.SecondQuestion4L0;
                    item.SecondQuestion5 = model.SecondQuestion5L0;
                    item.SecondQuestion6 = model.SecondQuestion6L0;
                    
                    item.ThirdQuestion = model.ThirdQuestionL0;
                    item.FourthQuestion = model.FourthQuestionL0;
                    _repoNewAttitudeEvaluation.Update(item);
                }
            }
            
            try
            {
                
                await _repoNewAttitudeEvaluation.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
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
