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
    public interface IAccountService
    {
        Task<object> AddAsync(AccountDto model);
        Task<OperationResult> UpdateAsync(AccountDto model);
        Task<OperationResult> LockAsync(int id);
        Task<OperationResult> UpdateL0Async(int id);
        Task<OperationResult> UpdateGhrAsync(int id);
        Task<OperationResult> UpdateGmAsync(int id);
        Task<OperationResult> UpdateGmScoreAsync(int id);
        Task<List<AccountDto>> GetAllAsync(string lang);
        Task<OperationResult> DeleteAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<OperationResult> ChangePasswordAsync(ChangePasswordRequest request);
        Task<object> ChangePasswordAsync2(ChangePasswordRequest request);
        Task<object> GetAccounts();
    }
    
}
