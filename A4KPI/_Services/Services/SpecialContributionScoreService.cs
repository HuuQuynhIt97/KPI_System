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
   
    public class SpecialContributionScoreService : ISpecialContributionScoreService
    {
        private OperationResult operationResult;
        private readonly ISpecialContributionScoreRepository _repo;
        private readonly IPerfomanceEvaluationTypeRepository _repoPerfomanceType;
        private readonly IPerfomanceEvaluationImpactRepository _repoPerfomanceImpact;
        private readonly ISpecialTypeRepository _repoSpecialType;
        private readonly ISpecialCompactRepository _repoSpecialCompact;
        private readonly ISpecialRatioRepository _repoSpecialRatio;
        private readonly ISpecialScoreRepository _repoSpecialScore;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public SpecialContributionScoreService(
            ISpecialContributionScoreRepository repo,
            IPerfomanceEvaluationTypeRepository repoPerfomanceType,
            IPerfomanceEvaluationImpactRepository repoPerfomanceImpact,
            ISpecialTypeRepository repoSpecialType,
            ISpecialCompactRepository repoSpecialCompact,
            ISpecialScoreRepository repoSpecialScore,
            ISpecialRatioRepository repoSpecialRatio,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoPerfomanceType = repoPerfomanceType;
            _repoPerfomanceImpact = repoPerfomanceImpact;
            _repoSpecialType = repoSpecialType;
            _repoSpecialCompact = repoSpecialCompact;
            _repoSpecialRatio = repoSpecialRatio;
            _repoSpecialScore = repoSpecialScore;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<SpecialContributionScore> GetSpecialScoreDetail(int campaignID, int scoreFrom, int scoreTo, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreFrom == scoreFrom
                && x.ScoreTo == scoreTo
                && x.ScoreType == type
            ).Select( x => new SpecialContributionScore {
                ID = x.ID,
                CampaignID = x.CampaignID,
                CompactID = x.CompactID,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                IsSubmit = x.IsSubmit,
                ModifiedTime = x.ModifiedTime,
                Point = x.Point == "1000" ? null : x.Point,
                Ratio = x.Ratio == "1000" ? null : x.Ratio,
                ScoreBy = x.ScoreBy,
                ScoreFrom = x.ScoreFrom,
                ScoreTo = x.ScoreTo,
                ScoreType = x.ScoreType,
                SpecialScore = x.SpecialScore,
                Subject = x.Subject,
                TypeID = x.TypeID
            }).FirstOrDefaultAsync();
            
            return result;
        }

        public async Task<SpecialContributionScore> GetSpecialL1ScoreDetail(int campaignID, int userID, string type)
        {
            var result = await _repo.FindAll(
                x => x.CampaignID == campaignID
                && x.ScoreTo == userID
                && x.ScoreType == type
            ).FirstOrDefaultAsync();
            return result;
        }
        public async Task<bool> AddAsync(SpecialContributionScoreDto model)
        {
            var item = _repo.FindAll(
                x => x.CampaignID == model.CampaignID
                && x.ScoreBy == model.ScoreFrom
                && x.ScoreTo == model.ScoreTo
                && x.ScoreType == model.ScoreType
                ).FirstOrDefault();
            if (item == null)
            {
                var add = _mapper.Map<SpecialContributionScore>(model);
                add.CreatedTime = DateTime.Now;
                _repo.Add(add);

                // add type
                var type = new List<PerfomanceEvaluationType>();
                if (model.TypeListID.Count > 0)
                {
                    foreach (var item_type in model.TypeListID)
                    {
                        var item_add = new PerfomanceEvaluationType
                        {
                            CampaignID = model.CampaignID,
                            ScoreFrom = model.ScoreFrom,
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
                            ScoreFrom = model.ScoreFrom,
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
                && x.ScoreFrom == model.ScoreFrom
                && x.ScoreTo == model.ScoreTo
                && x.Type == model.ScoreType).ToList();

                if (item_del_type.Count > 0)
                {
                    _repoPerfomanceType.RemoveMultiple(item_del_type);
                    await _repoPerfomanceType.SaveAll();
                }
                // xoa impact

                // xoa type
                var item_del_impact = _repoPerfomanceImpact.FindAll(x => x.CampaignID == model.CampaignID
                && x.ScoreFrom == model.ScoreFrom
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
                            ScoreFrom = model.ScoreFrom,
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
                            ScoreFrom = model.ScoreFrom,
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
                _repo.Update(item);
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
        public async Task<List<SpecialContributionScoreDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<SpecialContributionScoreDto>(_configMapper)
                .OrderByDescending(x => x.ID).ToListAsync();
        }
        public async Task<OperationResult> UpdateAsync(SpecialContributionScoreDto model)
        {
            var update = _mapper.Map<SpecialContributionScore>(model);
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

        public async Task<List<SpecialTypeDto>> GetSpecialType(string lang)
        {
            return await _repoSpecialType.FindAll().ProjectTo<SpecialTypeDto>(_configMapper)
                .Select(x => new SpecialTypeDto { 
                    ID = x.ID,
                    Name = lang == Constants.SystemLang.EN ? x.Name : x.NameZh
                })
               .OrderByDescending(x => x.ID).ToListAsync();
        }

        public async Task<List<SpecialCompactDto>> GetSpecialCompact(string lang)
        {
            return await _repoSpecialCompact.FindAll().ProjectTo<SpecialCompactDto>(_configMapper)
                .Select(x => new SpecialCompactDto
                {
                    ID = x.ID,
                    Name = lang == Constants.SystemLang.EN ? x.Name : x.NameZh
                })
               .OrderByDescending(x => x.ID).ToListAsync();
        }

        public async Task<List<SpecialRatioDto>> GetSpecialRatio()
        {
            return await _repoSpecialRatio.FindAll().ProjectTo<SpecialRatioDto>(_configMapper)
              .ToListAsync();
        }

        public async Task<List<SpecialScoreDto>> GetSpecialScore()
        {
            return await _repoSpecialScore.FindAll().ProjectTo<SpecialScoreDto>(_configMapper)
              .ToListAsync();
        }


        public async Task<object> GetMultiType(int campaignID,  int scoreTo, string type)
        {
            //throw new NotImplementedException();
            var query = new List<int>();
            query = _repoPerfomanceType.FindAll(x => x.CampaignID == campaignID
                && x.ScoreTo == scoreTo
                && x.Type == type).Select(x => x.TypeID.ToInt()).ToList();
            if (query.Count == 0)
            {
                query = _repo.FindAll(x => x.CampaignID == campaignID
                && x.ScoreTo == scoreTo
                && x.ScoreType == type).Select(x => x.TypeID).ToList();
            }

            return query;
        }

        public async Task<object> GetMultiImpact(int campaignID, int scoreTo, string type)
        {
            var query = new List<int>();
            query = _repoPerfomanceImpact.FindAll(x => x.CampaignID == campaignID
                && x.ScoreTo == scoreTo
                && x.Type == type).Select(x => x.ImpactID.ToInt()).ToList();
            if (query.Count == 0)
            {
                query = _repo.FindAll(x => x.CampaignID == campaignID
                && x.ScoreTo == scoreTo
                && x.ScoreType == type).Select(x => x.CompactID).ToList();
            }

            return query;
        }
    }
}
