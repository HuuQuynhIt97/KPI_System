using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using A4KPI.Helpers;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Interface
{
    public interface IAccountGroupService
    {
        Task<List<AccountGroupDto>> GetAllAsync();
        Task<List<AccountGroupDto>> GetAccountGroupForTodolistByAccountId();

    }
    
}
