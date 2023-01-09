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
using NetUtility;
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Interface
{
    public interface INewAttitudeScoreService
    {
        Task<List<NewAttitudeContentDto>> GetAllAsync(int campaignID, int scoreTo, int scoreFrom);
        Task<OperationResult> UpdatePointAsync(int id, string point);
        Task<OperationResult> UpdateCommentAsync(NewAttitudeAttchmentDto model);

        Task<NewAttitudeEvaluationDto> GetAttEvaluationAsync(int campaignID, int scoreTo, int scoreFrom);
        Task<OperationResult> UpdateAttEvaluationAsync(NewAttitudeEvaluationDto model);
        Task<AttitudeSubmitDto> GetNewAttitudeSubmit(int campaignID, int scoreTo);
        Task<OperationResult> CheckSubmitNewAtt(int campaignID, int scoreTo, int scoreFrom, string type);
        Task<OperationResult> GenerateNewAttitudeScore(int campaignID, int scoreTo, int scoreFrom);
        Task<List<EvaluationDto>> GetSelfAppraisal(int userID);
        Task<List<EvaluationDto>> GetFirstLevelAppraisal(int userID);
        Task<List<EvaluationDto>> GetSecondLevelAppraisal(int userID);
        Task<List<EvaluationDto>> GetFLFeedback(int userID);
        Task<List<NewAttitudeDetailDto>> GetDetailNewAttitude(int campaignID, int scoreTo);
        Task<NewAttitudeEvaluationDetailDto> GetDetailNewAttitudeEvaluation(int campaignID, int scoreTo);
    }
    
}
