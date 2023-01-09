using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using NetUtility;
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Interface
{
    public interface IAttitudeScoreService
    {
        Task<object> AddAsync(AttitudeScoreDto model);
        Task<object> UpdateAttitudeSubmit(int campaignId, int submitTo, string currentKPI, string currentAtt);
        Task<object> GetAllScoreStation();
        Task<OperationResult> SaveScore(SaveScoreDto model);
        Task<OperationResult> SubmitAttitude(SaveScoreDto model);
        Task<object> UpdateAsync(AttitudeScoreDto model);
        Task<bool> UpdateReviseStation(ReviseStationDto model);
        Task<OperationResult> DeleteAsync(int id);
        Task<object> GetAllAsync(int campaignID, int userFrom, int userTo, string type);
        Task<object> GetPoint(string from, string to);
        Task<object> GetKPISelfScoreDefault(int campaignID, int userID, string type);
        Task<object> GetKPISelfScoreString(int campaignID, int userID, string type);
        Task<object> GetDetail(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailPassion(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailAccountbility(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailAttention(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailContinuous(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailEffective(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetDetailResilience(int campaignID, int flUser, int l0User, int l1User, int l2User, string type);
        Task<object> GetListCheckBehavior(int campaignID, int userFrom, int userTo, string type);
        Task<List<AttitudeScoreDto>> GetAllByCampaignAsync(int campaignId);
      
    }
    
}
