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
    public interface ICoreCompetenciesService
    {
        Task<List<NewCoreCompetenciesDto>> GetAllNewCoreCompetencies(string lang, int campaignID);
        Task<List<NewCoreCompetenciesDto>> GetNewCoreCompetencies(string lang, int campaignID);
        Task<List<NewCoreCompetenciesDto>> GetNewCoreCompetenciesScoreEquals2(string lang, int campaignID);
        // Task<List<NewCoreCompetenciesDto>> GetNewCoreCompetenciesScoreThan2(string lang, int campaignID);
        Task<object> GetNewCoreCompetenciesScoreThan2(string lang, int campaignID);
        Task<List<CoreCompetenciesAverageDto>> GetNewCoreCompetenciesAverage(string lang, int campaignID);
        Task<List<CoreCompetenciesPercentileDto>> GetNewCoreCompetenciesPercentile(string lang, int campaignID);
        Task<byte[]> ExportExcelNewCoreCompetencies(string lang, int campaignID);
        
        Task<List<CoreCompetenciesDto>> GetAllCoreCompetencies(string lang, int campaignID);
        Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreEquals2(string lang, int campaignID);
        Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreThan2(string lang, int campaignID);
        Task<List<CoreCompetenciesDto>> GetAllCoreCompetenciesScoreThan3(string lang, int campaignID);
        Task<List<CoreCompetenciesAverageDto>> GetAllCoreCompetenciesAverage(string lang, int campaignID);
        Task<List<CoreCompetenciesPercentileDto>> GetAllCoreCompetenciesPercentile(string lang, int campaignID);
        Task<List<CoreCompetenciesAttitudeBehaviorDto>> GetAllCoreCompetenciesAttitudeBehavior(string lang, int campaignID);
        Task<byte[]> ExportExcelCoreCompetencies(string lang, int campaignID);
    }
    
}
