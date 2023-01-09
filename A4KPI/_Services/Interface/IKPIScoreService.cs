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
    public interface IKPIScoreService
    {
        Task<OperationResult> UpdateAsync(KPIScoreDto model);
        Task<List<KPIScoreDto>> GetAllAsync();
        Task<KPIScore> GetKPIScoreDetail(int campaignID, int userID, string type);
        Task<KPIScore> GetKPIScoreL2L1Detail(int campaignID, int userID, string type);
        Task<KPIScore> GetKPIScoreL1L0Detail(int campaignID, int scoreFrom, int scoreTo, string type);
        Task<KPIScore> GetKPIScoreGMDetail(int campaignID, int scoreFrom, int scoreTo, string type);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> AddAsync(KPIScoreDto model);
        Task<OperationResult> SubmitKPI(KPIScoreDto model);
    }
    
}
