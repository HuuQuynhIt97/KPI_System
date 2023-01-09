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

namespace A4KPI._Services.Interface
{
    public interface ISpecialContributionScoreService
    {
        Task<OperationResult> UpdateAsync(SpecialContributionScoreDto model);
        Task<List<SpecialContributionScoreDto>> GetAllAsync();
        Task<List<SpecialTypeDto>> GetSpecialType(string lang);
        Task<List<SpecialCompactDto>> GetSpecialCompact(string lang);
        Task<List<SpecialRatioDto>> GetSpecialRatio();
        Task<List<SpecialScoreDto>> GetSpecialScore();
        Task<SpecialContributionScore> GetSpecialScoreDetail(int campaignID, int scoreFrom, int scoreTo, string typ);
        Task<SpecialContributionScore> GetSpecialL1ScoreDetail(int campaignID, int userID,string type);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> AddAsync(SpecialContributionScoreDto model);

        Task<object> GetMultiType(int campaignID, int scoreTo, string type);
        Task<object> GetMultiImpact(int campaignID,  int scoreTo, string type);
    }
    
}
