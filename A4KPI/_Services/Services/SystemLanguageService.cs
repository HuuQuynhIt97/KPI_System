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
using A4KPI.Helpers;
using A4KPI._Services.Interface;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using Microsoft.AspNetCore.Http;

namespace A4KPI._Services.Services
{
   
    public class SystemLanguageService :  ISystemLanguageService
    {
        private readonly ISystemLanguageRepository _repo;
        private readonly IAccountRepository _repoAccount;
        private readonly IAccountTypeRepository _repoAccountType;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public SystemLanguageService(
            ISystemLanguageRepository repo,
            IAccountRepository repoAccount,
            IHttpContextAccessor httpContextAccessor,
            IAccountTypeRepository repoAccountType, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _repoAccountType = repoAccountType;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> Add(Models.SystemLanguage model)
        {
            if (model.SLKey.Length > 50)
            {
                return new 
                {
                    Status = false,
                    Message = "KEY_LANGUAGE_TOO_LONG_MESSAGE"
                };
            }
            var query = _repo.FindAll(x => x.SLKey == model.SLKey).FirstOrDefault();
            if (query != null)
            {
                return new 
                {
                    Status = false,
                    Message = "LANGUAGE_ADD_DUPLICATE_MESSAGE"
                };
            }
               
            _repo.Add(model);
            await _repo.SaveAll();
            return new 
                {
                    Status = true,
                    Message = "LANGUAGE_ADD_SUCCESS_MESSAGE"
                };
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.FindById(id);
            _repo.Remove(item);
            return await _repo.SaveAll();
        }

        public async Task<object> GetLanguages(string lang)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = 0;
            if (token != null)
            {
                accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            }
            // var account = await _repoAccount.FindByIdAsync(accountId);
            // var accountType = await _repoAccountType.FindByIdAsync(account.AccountType);
            var query = _repoAccount.FindAll(x => x.Id == accountId && x.AccountType.Code == Systems.SupremeAdmin).FirstOrDefault();

            var data = (await _repo.FindAll().AsNoTracking().Select(x => new {
                x.SLKey,
                x.SLType,
                Name = lang == "zh" ? x.SLTW : x.SLEN
            }).ToListAsync()).DistinctBy(x => x.SLKey);
            var languages = data.ToDictionary(t => t.SLKey, t => t.Name);
            if (query != null)
            {
                languages = data.ToDictionary(t => t.SLKey, t => t.SLKey);
            }
            return languages;
        }

        public Models.SystemLanguage GetById(int id) => _repo.FindById(id);

        public async Task<PagedList<Models.SystemLanguage>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repo.FindAll().OrderByDescending(x => x.ID);
            return await PagedList<Models.SystemLanguage>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<PagedList<Models.SystemLanguage>> Search(PaginationParams param, object text)
        {
            var lists = _repo.FindAll()
            .OrderByDescending(x => x.ID);
            return await PagedList<Models.SystemLanguage>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<object> Update(Models.SystemLanguage model)
        {
            if (model.SLKey.Length > 50)
            {
                return new 
                {
                    Status = false,
                    Message = "KEY_LANGUAGE_TOO_LONG_MESSAGE"
                };
            }
            var query = _repo.FindAll(x => x.ID != model.ID && x.SLKey == model.SLKey).FirstOrDefault();
            if (query != null)
            {
                return new 
                {
                    Status = false,
                    Message = "KEY_LANGUAGE_DUPLICATE_MESSAGE"
                };
            }
               
            _repo.Update(model);
            await _repo.SaveAll();
            return new 
                {
                    Status = true,
                    Message = "LANGUAGE_UPDATE_SUCCESS_MESSAGE"
                };
            
        }

        public Task<bool> UpdateLanguage()
        {
            //var data = new List<SystemLanguage>();
            //string json = System.IO.File.ReadAllText(@"wwwroot/Language/" + "en.json");
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //foreach (var items in jsonObj)
            //{
            //    var data2 = new SystemLanguage();
            //    data2.Slkey = items.Key;
            //    data2.Slen = items.Value;
            //    data.Add(data2);
            //}
            //_repo.AddRange(data);
            //return _repo.SaveAll();

            //var dataExist = _repo.FindAll().ToList();
            //var data = new List<SystemLanguage>();
            //string json = System.IO.File.ReadAllText(@"wwwroot/Language/" + "zh.json");
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //foreach (var item in dataExist)
            //{
            //    foreach (var items in jsonObj)
            //    {
            //        if (item.Slkey == items.Key)
            //        {
            //            item.Sltw = items.Value;
            //        }
            //    }
            //}
            //_repo.UpdateRange(dataExist);

            string root = @"C:\users";
            string root2 = @"C:\Users";

            bool result = root.Equals(root2);
            Console.WriteLine($"Ordinal comparison: <{root}> and <{root2}> are {(result ? "equal." : "not equal.")}");

            result = root.Equals(root2, StringComparison.Ordinal);
            Console.WriteLine($"Ordinal comparison: <{root}> and <{root2}> are {(result ? "equal." : "not equal.")}");

            Console.WriteLine($"Using == says that <{root}> and <{root2}> are {(root == root2 ? "equal" : "not equal")}");
            return _repo.SaveAll();
        }

        public async Task<object> GetAllAsync()
        {
            return await _repo.FindAll().OrderByDescending(X => X.ID).ToListAsync();
            throw new NotImplementedException();
        }
    }
}
