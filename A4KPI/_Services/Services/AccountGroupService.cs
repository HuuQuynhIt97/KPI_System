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
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
    
    public class AccountGroupService : IAccountGroupService
    {
        private readonly IAccountGroupRepository _repo;
        private readonly IAccountGroupAccountRepository _repoAccount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupService(
            IAccountGroupRepository repo,
            IAccountGroupAccountRepository repoAccount,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }

       

        public async Task<List<AccountGroupDto>> GetAccountGroupForTodolistByAccountId()
        {
            var currentMonth = DateTime.Today.Month;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
           
            // tim oc cua usser login
            return await _repoAccount.FindAll(x => x.AccountId == accountId)
                .Where(x=> x.AccountGroup.Position != 100)
                .Select(x=>x.AccountGroup)
                .ProjectTo<AccountGroupDto>(_configMapper).ToListAsync();
        }

        public async Task<List<AccountGroupDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<AccountGroupDto>(_configMapper)
                .OrderByDescending(x => x.Id).ToListAsync();
        }

       
    }
}
