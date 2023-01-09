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
    public interface IPeopleCommitteeService
    {
        Task<List<PeopleCommitteeDto>> GetAll(string lang, int campaignID);
        Task<PeopleCommitteeDto> GetPeopleCommittee(int appraiseeID);
        Task<object> GetKpi(int accountID);
        Task<object> GetAllKpiScore(int scoreTo, int campaignID);
        Task<object> GetAllAttitudeScore(int scoreTo, int campaignID);
        Task<object> GetSumAttitudeScore(int scoreTo, int campaignID);
        Task<object> GetSumNewAttitudeScore(int scoreTo, int campaignID);
        Task<NewAttitudeEvaluationDetailDto> GetDetailNewAttitudeEvaluation(int scoreTo, int campaignID);
        Task<object> GetSpecialScoreDetail(int scoreTo, int campaignID);
        Task<List<PeopleCommitteeSpecialScoreDto>> GetScoreL2(int scoreTo, int campaignID);
        Task<CommitteeScoreDto> GetCommitteeScore(int scoreTo, int scoreFrom, int campaignID);
        Task<bool> GetFrozen(int campaignID);
        Task<OperationResult> UpdateKpiScore(KPIScoreDto model);
        Task<OperationResult> UpdateAttitudeScore(PeopleCommitteeAttScoreDto model);
        Task<OperationResult> UpdateSpecialScore(SpecialContributionScoreDto model);
        Task<bool> UpdateSpecialContribution(SpecialContributionScoreDto model);
        Task<OperationResult> UpdateKpiScoreL2(PeopleCommitteeSpecialScoreDto model);
        Task<OperationResult> UpdateCommitteeScore(CommitteeScoreDto model);
        Task<OperationResult> UpdateCommitteeSequence(CommitteeSequenceDto model);
        Task<OperationResult> UpdateNewAttitudeEvaluation(NewAttitudeEvaluationDetailDto model);
        Task<OperationResult> LockUpdate(int campaignID);

        Task<object> GetKPIDefaultPerson(int campaignID, int userID);
        Task<object> GetKPIStringPerson(int campaignID, int userID);
        Task<object> GetKPIDefaultMuti(int campaignID, int userID);
        Task<object> GetKPIStringMuti(int campaignID, int userID);
    }
    
}
