using AutoMapper;
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
using AutoMapper.QueryableExtensions;
using A4KPI.Constants;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;
using A4KPI.Helpers;
using System.Collections;

namespace A4KPI._Services.Services
{

    public class KPIMonthPerfService : IKPIMonthPerfService
    {
        private readonly IAccountRepository _repoAc;
        private readonly IAccountTypeRepository _repoAcType;

        private readonly IKPINewRepository _repoKPINew;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IDoRepository _repoDo;
        private readonly IActionStatusRepository _repoAcs;
        private readonly IOCRepository _repoOC;
        private readonly ITypeRepository _repoType;
        private readonly ITargetRepository _repoTarget;
        private readonly ITargetYTDRepository _repoTargetYTD;
        private readonly IActionRepository _repoAction;
        private readonly IStatusRepository _repoStatus;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public KPIMonthPerfService(
            IOCRepository repoOC,
            ITypeRepository repoType,
            IKPINewRepository repoKPINew,
            IKPIAccountRepository repoKPIAc,
            ITargetRepository repoTarget,
            ITargetYTDRepository repoTargetYTD,
            IActionStatusRepository repoAcs,
            IAccountRepository repoAc,
            IAccountTypeRepository repoAcType,
            IDoRepository repoDo,
            IActionRepository repoAction,
            IStatusRepository repoStatus,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repoOC = repoOC;
            _repoAc = repoAc;
            _repoAcType = repoAcType;
            _repoAcs = repoAcs;
            _repoType = repoType;
            _repoTarget = repoTarget;
            _repoTargetYTD = repoTargetYTD;
            _repoKPINew = repoKPINew;
            _repoKPIAc = repoKPIAc;
            _repoDo = repoDo;
            _repoAction = repoAction;
            _repoStatus = repoStatus;
            _mapper = mapper;
            _configMapper = configMapper;
        }



        public async Task<KPIMonthPerfAllDto> GetAllKPI(int userId, DateTime time)
        {
            var currentTime = time;
            var current_year = time.Year;
            var current_month = time.Month;
            var data = new List<KPIMonthPerfDto>();
            var result = new List<KPIMonthPerfDto>();
            var user = _repoAc.FindById(userId);
            var isAdmin = _repoAcType.FindById(user.AccountTypeId).Code;
            var fact_name = _repoOC.FindById(user.FactId) != null ? _repoOC.FindById(user.FactId).Name : "";
            var center_name = _repoOC.FindById(user.CenterId) != null ? _repoOC.FindById(user.CenterId).Name : "";
            var dept_name = _repoOC.FindById(user.DeptId) != null ? _repoOC.FindById(user.DeptId).Name : "";

            data = (from x in (await _repoKPINew.FindAll().OrderBy(x => x.Name).ToListAsync())
                   join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics
                   let fact = pics.Select(x =>
                        _repoOC.FindById(x.FactId) != null ? _repoOC.FindById(x.FactId).Name : null)
                    let center = pics.Select(x =>
                        _repoOC.FindById(x.CenterId) != null ? _repoOC.FindById(x.CenterId).Name : null)
                    let dept = pics.Select(x =>
                        _repoOC.FindById(x.DeptId) != null ? _repoOC.FindById(x.DeptId).Name : null)
                    let target = _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault() != null ? _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault().Value : 0

                    let performance = _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault() != null ? _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault().Performance : 0


                    let ytd = _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault() != null ? _repoTarget.FindAll(o => o.KPIId == x.Id && o.TargetTime.Month == current_month && o.TargetTime.Year == current_year)
                    .FirstOrDefault().YTD : 0

                    select new KPIMonthPerfDto
                    {

                    Id = x.Id,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Status = x.IsDisplayTodo,
                    Year = !x.Year.IsNullOrEmpty() ? x.Year : x.CreatedTime.Year.ToString(),
                    TypeId = x.TypeId,
                    TypeName = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Name,
                    Level = x.Level,
                    CreatedTime = x.CreatedTime,
                    Sequence = x.Sequence,
                    Target = target.ToString() + "%",
                    Performance = performance.ToString() + "%",
                    YTD = ytd.ToString() + "%",
                    PICNum = pics.Select(x => x.AccountId).ToList(),
                    TypeText = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Description,
                    PICName = _repoKPIAc.FindAll(y => y.KpiId == x.Id).ToList().Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                    FactName = String.Join(" , ", fact.Where(x => !String.IsNullOrEmpty(x))),
                    CenterName = String.Join(" , ", center.Where(x => !String.IsNullOrEmpty(x))),
                    DeptName = String.Join(" , ", dept.Where(x => !String.IsNullOrEmpty(x))),
                    }).OrderBy(x => x.Level).ToList();
            var model = data.Select(x => new KPIMonthPerfDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Status = x.Status,
                PICName = x.PICName,
                TypeId = x.TypeId,
                Level = x.Level - 1,
                PICNum = x.PICNum,
                Target = x.Target,
                Performance = x.Performance,
                YTD = x.YTD,
                CreatedTime = x.CreatedTime,
                Sequence = x.Sequence,
                Year = x.Year,
                TypeName = x.TypeName,
                TypeText = x.TypeText,
                FactName = x.FactName,
                CenterName = x.CenterName,
                DeptName = x.DeptName,

            }).OrderBy(x => x.Sequence).ToList().AsHierarchy(x => x.Id, y => y.ParentId);

            foreach (var item in model.Select(x => x.ChildNodes))
            {
                var result_tamp = item.ToList();
                foreach (var item_sub in result_tamp)//level1
                {
                    result.Add(item_sub.Entity);
                    foreach (var item_sub_child in item_sub.ChildNodes)//level2
                    {
                        result.Add(item_sub_child.Entity);
                        foreach (var item_sub_childs in item_sub_child.ChildNodes)//level3
                        {
                            result.Add(item_sub_childs.Entity);
                        }
                    }
                }
                
            }
             return new KPIMonthPerfAllDto
             {
                result = result.Where(x => x.Year.Contains(current_year.ToString()) && x.Status == false).ToList(),
             
            };
        }

        
    }
}
