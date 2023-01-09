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
    public interface IEvaluationService
    {
        Task<OperationResult> UpdateAsync(EvaluationDto model);
        Task<List<EvaluationDto>> GetAllAsync();
        Task<List<EvaluationDto>> GetSelfAppraisal(int userID);
        Task<List<EvaluationDto>> GetFirstLevelAppraisal(int userID);
        Task<List<EvaluationDto>> GetSecondLevelAppraisal(int userID);
        Task<List<EvaluationDto>> GetFLFeedback(int userID);
        Task<List<EvaluationDto>> GetGMData(int userID);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> AddAsync(EvaluationDto model);
        Task<OperationResult> GenerateEvaluation(int campaignID);
        Task<OperationResult> GenerateAttitudeSubmit(int campaignID);
    }
    
}
