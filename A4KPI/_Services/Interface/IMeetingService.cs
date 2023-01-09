﻿using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NetUtility;
using AutoMapper.QueryableExtensions;
using A4KPI.Constants;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Interface
{
    public interface IMeetingService 
    {
        Task<MeetingAllDto> GetAllKPI(int userId);
        Task<MeetingAllDto> GetAllKpiCHM(int userId);
        Task<object> GetAllLevel();
        Task<List<MeetingDto>> GetAllKPIWithFilterQuery(MeetingFilterRequest request);
        Task<ChartDtoDateTime> GetChartWithDateTime(int kpiId, DateTime time);
    }
    
}
