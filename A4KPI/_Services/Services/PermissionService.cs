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
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
   
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IOptionFunctionSystemRepository _repoOptionFunctionSystem;
        private readonly IOCRepository _repoOc;
        private readonly IRoleRepository _repoRole;
        private readonly IOptionRepository _repoOption;
        private readonly IFunctionTranslationRepository _repoFunctionTranslation;
        private readonly IAccountGroupAccountRepository _repoAccountGroupAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        private readonly string[] Permissions = new string[] { "Action", "Action In Function", "Module", "Function" };
        public PermissionService(
            IPermissionRepository repo,
            IOCRepository repoOC,
            IRoleRepository repoRole,
            IOptionRepository repoOption,
            IFunctionTranslationRepository repoFunctionTranslation,
            IOptionFunctionSystemRepository repoOptionFunctionSystem,
            IUserRoleRepository repoUserRole,
            IAccountGroupAccountRepository repoAccountGroupAccount,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOc = repoOC;
            _repoRole = repoRole;
            _repoOption = repoOption;
            _repoFunctionTranslation = repoFunctionTranslation;
            _repoOptionFunctionSystem = repoOptionFunctionSystem;
            _repoUserRole = repoUserRole;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public async Task<object> GetActionInFunctionByRoleID(int roleID)
        {
            var query = _repo.FindAll(x => x.RoleID == roleID)
                .Include(x => x.Functions)
                .Include(x => x.Option)
                .Select(x => new
                {
                    x.Functions.Name,
                    FunctionCode = x.Functions.Code,
                    x.Functions.Url,
                    x.Option.Code,
                    x.OptionID
                });
            var data = (await query.ToListAsync()).GroupBy(x => new { x.Name, x.FunctionCode, x.Url })
                    .Select(x => new
                    {
                        x.Key.Name,
                        x.Key.FunctionCode,
                        x.Key.Url,
                        Childrens = x
                    });
            return data;
        }

        public async Task<object> GetMenuByLangID(int userId, string langID)
        {
            var roles = await _repoUserRole.FindAll(x => x.UserID == userId).Select(x => x.RoleID).ToArrayAsync();

            var query = from p in _repo.FindAll()
                        join f in _repoFunctionTranslation.FindAll(x => x.LanguageID.Equals(langID))
                                .Include(x => x.FunctionSystem)
                                .ThenInclude(x => x.Module)
                                .ThenInclude(x => x.ModuleTranslations)
                        on p.FunctionSystemID equals f.FunctionSystemID
                        join r in _repoRole.FindAll() on p.RoleID equals r.ID
                        join a in _repoOption.FindAll() on p.OptionID equals a.ID
                        where roles.Contains(r.ID) && a.Code == "VIEW"
                        select new 
                        {
                            Id = f.ID,
                            Name = f.Name,
                            Code = f.FunctionSystem.Code,
                            Url = f.FunctionSystem.Url,
                            Icon = f.FunctionSystem.Icon,
                            ParentId = f.FunctionSystem.ParentID,
                            SortOrder = f.FunctionSystem.Sequence,
                            Module = f.FunctionSystem.Module,
                            ModuleId = f.FunctionSystem.ModuleID
                        };
            var data = query.ToList().Distinct().OrderBy(x => x.SortOrder).ToList();
            return data.GroupBy(x => x.Module).Select(x => new
            {
                Module = x.Key.ModuleTranslations.Count > 0 ?
                x.Key.ModuleTranslations.FirstOrDefault(x => x.LanguageID.Equals(langID)).Name
                : x.Key.Name,
                Icon = x.Key.Icon,
                Url = x.Key.Url,
                Sequence = x.Key.Sequence,
                Children = x,
                HasChildren = x.Any()
            }).OrderBy(x => x.Sequence).ToList();
        }

        public async Task<object> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request)
        {

            var roleID = request.RoleIDs;
            var lang = request.lang;
            var permission = _repo.FindAll();
            var query = _repoOptionFunctionSystem.FindAll()
                .Include(x => x.Option)
                .Include(x => x.FunctionSystem)
                .ThenInclude(x => x.FunctionTranslations)
                .Include(x => x.FunctionSystem)
                .ThenInclude(x => x.Module)
                .Select(x => new
                {
                    Id = x.FunctionSystem.ID,
                    FunctionCode = x.FunctionSystem.Code,
                    Name = x.FunctionSystem.FunctionTranslations.First(x => x.LanguageID.Equals(lang)).Name,
                    ActionName = x.Option.Name,
                    ActionID = x.Option.ID,
                    SequenceFunction = x.FunctionSystem.Sequence,
                    Module = x.FunctionSystem.Module,
                    ModuleName = x.FunctionSystem.Module.ModuleTranslations.First(x => x.LanguageID.Equals(lang)).Name,
                    ModuleCode = x.FunctionSystem.Module.Code,
                    ModuleNameID = x.FunctionSystem.Module.ID,
                    Code = x.Option.Code,
                }).Where(x => !Permissions.Contains(x.FunctionCode));
            // Dieu kien nay de khong load nhung chuc nang he thong
            var model = from t1 in query
                        from t2 in permission.Where(x => roleID.Contains(x.RoleID) && t1.Id == x.FunctionSystemID && x.OptionID == t1.ActionID)
                            .DefaultIfEmpty()
                        select new
                        {
                            t1.Id,
                            t1.Name,
                            t1.ActionName,
                            t1.ActionID,
                            t1.ModuleName,
                            t1.Code,
                            t1.Module,
                            t1.SequenceFunction,
                            Permission = t2
                        };
            var data = (await model.ToListAsync())
                        .GroupBy(x => x.Module)
                        .Select(x => new
                        {

                            //ModuleName = x.Key.ModuleTranslations.First(x => x.LanguageID.Equals(lang)).Name,
                            ModuleName = x.First().ModuleName,
                            Sequence = x.Key.Sequence,
                            Fields = new
                            {
                                DataSource = x.GroupBy(s => new { s.Id, s.Name, s.SequenceFunction })
                                .Select(g => new
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name,
                                    SequenceFunction = g.Key.SequenceFunction,
                                    Childrens = g
                                    .Select(a => new
                                    {
                                        ParentID = g.Key.Id,
                                        ID = $"{a.ActionID}_{g.Key.Id}_{roleID.FirstOrDefault()}",
                                        Name = a.ActionName,
                                        a.ActionID,
                                        FunctionID = g.Key.Id,
                                        a.ActionName,
                                        Status = a.Permission != null,

                                    }).ToList()
                                }).OrderBy(x => x.SequenceFunction).ToList(),
                                Id = "id",
                                Text = "name",
                                Child = "childrens"
                            }
                        });
            return data.OrderBy(x => x.Sequence).ToList();
        }

        public async Task<ResponseDetail<object>> PutPermissionByRoleId(int roleID, UpdatePermissionRequest request)
        {

            try
            {
                //create new permission list from user changed
                var newPermissions = new List<Permission>();
                foreach (var p in request.Permissions)
                {
                    newPermissions.Add(new Permission(roleID, p.ActionID, p.FunctionID));
                }
                var existingPermissions = _repo.FindAll(x => x.RoleID == roleID).ToList();
                if (existingPermissions.Count > 0)
                {
                    _repo.RemoveMultiple(existingPermissions);

                }
                _repo.AddRange(newPermissions.DistinctBy(x => new { x.RoleID, x.OptionID, x.FunctionSystemID }).ToList());

                await _repo.SaveAll();
                return new ResponseDetail<object> { Status = true };
            }
            catch (System.Exception ex)
            {
                // TODO
                return new ResponseDetail<object> { Status = false, Message = ex.Message };
            }

            // tao role moi
        }

        
    }
}
