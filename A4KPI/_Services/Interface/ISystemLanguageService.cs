﻿using AutoMapper;
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

namespace A4KPI._Services.Interface
{
    public interface ISystemLanguageService
    {
        Task<object> GetLanguages(string lang);
        Task<object> GetAllAsync();
        Task<bool> UpdateLanguage();
        Task<PagedList<Models.SystemLanguage>> Search(PaginationParams param, object text);
        SystemLanguage GetById(int id);
        Task<object> Add(SystemLanguage model);
        Task<object> Update(SystemLanguage model);
        Task<bool> Delete(int id);
        Task<PagedList<Models.SystemLanguage>> GetWithPaginations(PaginationParams param);
    }
    
}
