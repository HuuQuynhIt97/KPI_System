using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using A4KPI.Helpers;
using A4KPI.Constants;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;

namespace A4KPI._Services.Services
{
 
    public class KPINewService : IKPINewService
    {
        private readonly IKPINewRepository _repo;
        private readonly IOCRepository _repoOc;
        private readonly ITypeRepository _repoType;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IAccountRepository _repoAc;
        private readonly IActionRepository _repoAction;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public KPINewService(
            IKPINewRepository repo,
            ITypeRepository repoType,
            IAccountRepository repoAc,
            IKPIAccountRepository repoKPIAc,
            IActionRepository repoAction,
            IOCRepository repoOc,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOc = repoOc;
            _repoKPIAc = repoKPIAc;
            _repoType = repoType;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _repoAction = repoAction;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> GetListPic()
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();

            var dataAc = _repoAc.FindById(accountId);
            var list = new List<AccountDto>();
            var list_account_default = (await _repoAc.FindAll().ProjectTo<AccountDto>(_configMapper).OrderBy(x => x.FullName).ToListAsync()).Select(x => new AccountDto
            {
                Id = x.Id,
                FullName = x.FullName,
                FactId = x.FactId,
                DeptId = x.DeptId,
                CenterId = x.CenterId
                
            }).ToList();
            var list_account_default_fater = list_account_default.Select(x => new AccountDto
            {
                Id = x.Id,
                FullName = x.FullName,
                FactId = x.FactId,
                DeptId = x.DeptId,
                CenterId = x.CenterId,
                Level = x.FactId > 0 && x.CenterId > 0 && x.DeptId > 0 
                ? _repoOc.FindById(x.DeptId) != null ? _repoOc.FindById(x.DeptId).Level : 0
                : x.FactId > 0 && x.CenterId > 0 && x.DeptId == 0 
                ? _repoOc.FindById(x.CenterId) != null ? _repoOc.FindById(x.CenterId).Level  : 0
                : x.FactId > 0 && x.CenterId == 0 && x.DeptId == 0 ?
                _repoOc.FindById(x.FactId) != null ? _repoOc.FindById(x.FactId).Level : 0 : 0
            }).ToList();

            if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
            {
                
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.FactId).Select(x => x.Id).ToList();
                list = list_account_default_fater.Where(x => OcIdUnder.Contains(x.CenterId.ToInt())).ToList();
            }

            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
            {
                //List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                var level_oc_user = _repoOc.FindById(dataAc.CenterId) != null ? _repoOc.FindById(dataAc.CenterId).Level : 0;
                //list = lists.Where(x => OcIdUnder.Contains(x.DeptId.ToInt()) || x.Id == accountId).ToList(); list Pic-Old
                list = list_account_default_fater.Where(x => x.Level >= level_oc_user).ToList();
            }

            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
            {
                //List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                //list = lists.Where(x => x.DeptId == dataAc.DeptId).ToList();
                var level_oc_user = _repoOc.FindById(dataAc.CenterId) != null ? _repoOc.FindById(dataAc.CenterId).Level : 0;
                list = list_account_default_fater.Where(x => x.Level >= level_oc_user).ToList();
            }

            var data = list.Select(x => new AccountDto { 
                Id = x.Id,
                FullName = x.FullName,
                Level = x.Level
            }).ToList();
            return data;
        }

        public async Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView(string lang)
        {
            var lists = new List<KPINewDto>();
            

            lists = (from x in (await _repo.FindAll().OrderBy(x => x.Name).ToListAsync())
                     join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics
                     let pic = pics.Select(a => a.AccountId).ToList()
                     let mgmt = pics.Select(x =>
                        x.FactId > 0 && x.CenterId > 0 && x.DeptId > 0 ? _repoOc.FindById(x.DeptId).Name
                        : x.FactId > 0 && x.CenterId > 0 && x.DeptId == 0 ? _repoOc.FindById(x.CenterId).Name
                        : x.FactId > 0 && x.CenterId == 0 && x.DeptId == 0 ? _repoOc.FindById(x.FactId).Name : null
                        )
                     let pic_name = pics.Select(x => _repoAc.FindById(x.AccountId).FullName)
                     join t in _repoType.FindAll() on x.TypeId equals t.Id into type_Name
                     let type_name = type_Name.FirstOrDefault()
                     join ac in _repoAc.FindAll() on x.UpdateBy equals ac.Id
                     select new KPINewDto
                    {
                         Id = x.Id,
                         ParentId = x.ParentId,
                         Name = x.Name,
                         IsDisplayTodo = x.IsDisplayTodo,
                         UpdateBy = x.UpdateBy,
                         Pics = pic.Count > 0 ? pic : new List<int> { },
                         CreateBy = x.CreateBy,
                         LevelOcCreateBy = x.LevelOcCreateBy,
                         OcIdCreateBy = x.OcIdCreateBy,
                         TypeId = x.TypeId,
                         Sequence = x.Sequence,
                         Year = x.Year != null ? x.Year : x.CreatedTime.Year.ToString(),
                         Level = x.Level,
                         StartDisplayMeetingTime = x.StartDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a" ?
                         x.StartDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                                 EndDisplayMeetingTime = x.EndDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a"
                         ? x.EndDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                         TypeName = x.TypeId == 0 ? "" : lang == SystemLang.EN ? type_name.NameEn : type_name.NameZh,
                         PICName = pic.Count > 0 ? String.Join(" , ", pic_name) : null,
                         UpdateDate = x.UpdateDate.ToString("MM/dd/yyyy"),
                         CreatedTimeYear = x.CreatedTime.Year.ToString(),
                         UpdateName = ac != null ? ac.FullName : "",
                         Mgmt = String.Join(" , ", mgmt)

                     }).OrderBy(x => x.Level).ToList();

            var data = lists.Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.Pics,
                TypeId = x.TypeId,
                CreateBy = x.CreateBy,
                Level = x.Level,
                IsDisplayTodo = x.IsDisplayTodo,
                Year = x.Year,
                Sequence = x.Sequence ?? 0,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                FactId = x.FactId,
                StartDisplayMeetingTime = x.StartDisplayMeetingTime,
                EndDisplayMeetingTime = x.EndDisplayMeetingTime,
                CenterId = x.CenterId,
                DeptId = x.DeptId,
                UpdateDate = x.UpdateDate,
                CreatedTimeYear = x.CreatedTimeYear,
                Mgmt = x.Mgmt

            }).OrderBy(x => x.Sequence).ToList().AsHierarchy(x => x.Id, y => y.ParentId);
            return data;
        }

        public async Task<object> GetAllAsTreeView2nd3rd(string lang , int userId)
        {
            var accountId = userId;
            var dataAc = _repoAc.FindById(accountId);
            var list = new List<KPINewDto>();
            var lists = new List<KPINewDto>();

            lists = (from x in (await _repo.FindAll().OrderBy(x => x.Name).ToListAsync())
                    join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics
                    let pic = pics.Select(a => a.AccountId).ToList()
                    let pic_name = pics.Select(x => _repoAc.FindById(x.AccountId).FullName)
                    let mgmt = pics.Select(x => 
                    x.FactId > 0 && x.CenterId > 0 && x.DeptId > 0 ? _repoOc.FindById(x.DeptId).Name 
                    : x.FactId > 0 && x.CenterId > 0 && x.DeptId == 0 ? _repoOc.FindById(x.CenterId).Name
                    : x.FactId > 0 && x.CenterId == 0 && x.DeptId == 0 ? _repoOc.FindById(x.FactId).Name : null
                    )
                    join t in _repoType.FindAll() on x.TypeId equals t.Id into type_Name
                    let type_name = type_Name.FirstOrDefault()
                    join ac in _repoAc.FindAll() on x.UpdateBy equals ac.Id
             select new KPINewDto
             {
                 Id = x.Id,
                 ParentId = x.ParentId,
                 Name = x.Name,
                 IsDisplayTodo = x.IsDisplayTodo,
                 UpdateBy = x.UpdateBy,
                 Pics = pic.Count > 0 ? pic : new List<int> { },
                 CreateBy = x.CreateBy,
                 LevelOcCreateBy = x.LevelOcCreateBy,
                 OcIdCreateBy = x.OcIdCreateBy,
                 TypeId = x.TypeId,
                 Sequence = x.Sequence,
                 Year = x.Year != null ? x.Year : x.CreatedTime.Year.ToString(),
                 Level = x.Level,
                 StartDisplayMeetingTime = x.StartDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a" ?
                 x.StartDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                 EndDisplayMeetingTime = x.EndDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a" 
                 ? x.EndDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null ,
                 TypeName = x.TypeId == 0 ? "" : lang == SystemLang.EN ? type_name.NameEn : type_name.NameZh,
                 PICName = pic.Count > 0 ? String.Join(" , ", pic_name) : null,
                 UpdateDate = x.UpdateDate.ToString("MM/dd/yyyy"),
                 CreatedTimeYear = x.CreatedTime.Year.ToString(),
                 UpdateName = ac != null ? ac.FullName : "",
                 Mgmt = String.Join(" , ", mgmt)

             }).OrderBy(x => x.Level).ToList();

            //Nếu account là admin normal
            if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
            {
                list = lists;
            }
            //Nếu account là Super Admin
            if (dataAc.FactId == 0 || dataAc.FactId == null && dataAc.CenterId == 0 || dataAc.CenterId == null && dataAc.DeptId == 0 || dataAc.DeptId == null)
            {
                list = lists;
            }

            //Nếu account là Manager
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                var OcIdOver = _repoOc.FindAll(x => x.Id == dataAc.CenterId).FirstOrDefault().ParentId;
                var picOver = _repoAc.FindAll(x => x.FactId == OcIdOver && x.CenterId == 0 && x.DeptId == 0 && x.Manager > 0).Select(x => x.Id).ToList();
                if (picOver.Count == 0)
                {
                    return false;
                }
                List<int> kpiPicOver = _repoKPIAc.FindAll(x => picOver.Contains(x.AccountId)).Select(x => x.KpiId).ToList();
                List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
                var tamp = lists.Where(
                    x => x.LevelOcCreateBy == Level.Level_1
                    || x.LevelOcCreateBy == Level.Level_2
                    || x.LevelOcCreateBy == Level.Level_3).ToList();
                List<int> kpiPicSame = new List<int>();

                var list_tamp = tamp.Where(
                    x => x.CreateBy == accountId
                    || kpiPicOver.Contains(x.Id)
                    || kpiMyPic.Contains(x.Id)
                    || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
                foreach (var item in list_tamp)
                {
                    if (item.Level == Level.Level_3)
                    {
                        var kpiSameID = tamp.Where(x => x.Id == item.ParentId).FirstOrDefault() != null ? tamp.Where(x => x.Id == item.ParentId).FirstOrDefault().Id : 0;
                        kpiPicSame.Add(kpiSameID);
                    }
                }
                list = tamp.Where(
                    x => x.CreateBy == accountId
                    || kpiPicOver.Contains(x.Id)
                    || kpiPicSame.DistinctBy(y => y).Contains(x.Id)
                    || kpiMyPic.Contains(x.Id)
                    || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
            }

            //Nếu account là Normal User
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                var OcIdOver = _repoOc.FindAll(x => x.Id == dataAc.CenterId).FirstOrDefault().ParentId;
                var picOver = _repoAc.FindAll(x => x.FactId == OcIdOver && x.CenterId == 0 && x.DeptId == 0 && x.Manager > 0).Select(x => x.Id).ToList();
                if (picOver.Count == 0)
                {
                    return false;
                }
                List<int> kpiPicOver = _repoKPIAc.FindAll(x => picOver.Contains(x.AccountId)).Select(x => x.KpiId).ToList();
                List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
                var tamp = lists.Where(
                    x => x.LevelOcCreateBy == Level.Level_1
                    || x.LevelOcCreateBy == Level.Level_2
                    || x.LevelOcCreateBy == Level.Level_3).ToList();
                List<int> kpiPicSame = new List<int>();

                var list_tamp = tamp.Where(
                    x => x.CreateBy == accountId
                    || kpiPicOver.Contains(x.Id)
                    || kpiMyPic.Contains(x.Id)
                    || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
                foreach (var item in list_tamp)
                {
                    if (item.Level == Level.Level_3)
                    {
                        var kpiSameID = tamp.Where(x => x.Id == item.ParentId).FirstOrDefault() != null ? tamp.Where(x => x.Id == item.ParentId).FirstOrDefault().Id : 0;
                        kpiPicSame.Add(kpiSameID);
                    }
                }
                list = tamp.Where(
                    x => x.CreateBy == accountId
                    || kpiPicOver.Contains(x.Id)
                    || kpiPicSame.DistinctBy(y => y).Contains(x.Id)
                    || kpiMyPic.Contains(x.Id)
                    || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
                //List<int> picOver = _repoAc.FindAll(
                //    x => x.FactId == dataAc.FactId 
                //    && x.CenterId == dataAc.CenterId 
                //    && x.DeptId == 0).Select(x => x.Id).ToList();
                //var kpiPicOver = new List<int>();
                //foreach (var item in picOver)
                //{
                //    var datas = _repoKPIAc.FindAll(x => x.AccountId == item).Select(x => x.KpiId).ToList();
                //    kpiPicOver.AddRange(datas);
                //}
                //List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
                //list = lists.Where(
                //    x => x.CreateBy == accountId 
                //    || kpiMyPic.Contains(x.Id) 
                //    || kpiPicOver.Contains(x.Id)).ToList();

                //foreach (var item in list)
                //{
                //    if (item.Level == Level.Level_2)
                //    {
                //        item.ParentId = null;
                //    }
                //}

            }

            var data = list.Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.Pics,
                TypeId = x.TypeId,
                CreateBy = x.CreateBy,
                Level = x.Level,
                IsDisplayTodo = x.IsDisplayTodo,
                Year = x.Year,
                Sequence = x.Sequence ?? 0,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                FactId = x.FactId,
                StartDisplayMeetingTime = x.StartDisplayMeetingTime,
                EndDisplayMeetingTime = x.EndDisplayMeetingTime,
                CenterId = x.CenterId,
                DeptId = x.DeptId,
                UpdateDate = x.UpdateDate,
                CreatedTimeYear = x.CreatedTimeYear,
                Mgmt = x.Mgmt
                

            }).OrderBy(x => x.Sequence).ToList().AsHierarchy(x => x.Id, y => y.ParentId);
            return data;
        }

        public async Task<object> GetAllType(string lang)
        {
            var data = new List<TypeDto>();

            data = await _repoType.FindAll().Select(x => new TypeDto
            {
                Id = x.Id,
                Name = lang == SystemLang.EN ? x.NameEn : x.NameZh,
                Description = x.Description
            }).ToListAsync();
            
            return data;
        }

        public  async Task<OperationResult> AddAsync(KPINewDto model)
        {
            try
            {
                var dataAc = _repoAc.FindById(model.UpdateBy);

                var levelCreateBy = 0;
                if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = Level.Level_1;
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = Level.Level_2;
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
                {
                    levelCreateBy = Level.Level_3;
                }

                var ocIdCreate = 0;
                if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
                {
                    ocIdCreate = dataAc.FactId.ToInt();
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
                {
                    ocIdCreate = dataAc.CenterId.ToInt();
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
                {
                    ocIdCreate = dataAc.DeptId.ToInt();
                }

                model.OcIdCreateBy = ocIdCreate;
                model.LevelOcCreateBy = levelCreateBy;
                model.CreateBy = model.UpdateBy;
                model.UpdateBy = model.UpdateBy;
                var item = _mapper.Map<KPINew>(model);
                item.UpdateDate = DateTime.Now;

                int id =  await AddKPINew(item);

                var list = new List<KPIAccount>();
                foreach (var acId in model.KpiIds)
                {
                    var dataAdd = await AddKPIAccount(acId, id);
                    list.Add(dataAdd);
                }
                _repoKPIAc.AddRange(list);

                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public  async Task<OperationResult> UpdateAsync(KPINewDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                item.Name = model.Name;
                item.PolicyId = model.PolicyId;
                item.TypeId = model.TypeId;
                item.UpdateBy = model.UpdateBy;
                item.Sequence = model.Sequence;
                item.UpdateDate = DateTime.Now;
                item.StartDisplayMeetingTime = Convert.ToDateTime(model.StartDisplayMeetingTime);
                item.EndDisplayMeetingTime = Convert.ToDateTime(model.EndDisplayMeetingTime);
                _repo.Update(item);

                if (model.KpiIds.Count == 0)
                {
                    var removingList = await _repoKPIAc.FindAll(x => x.KpiId == model.Id).ToListAsync();
                    _repoKPIAc.RemoveMultiple(removingList);
                }

                var list = new List<KPIAccount>();
                var list_exist = new List<KPIAccount>();
                var list_delete = new List<KPIAccount>();
                var list_account_remove = new List<KPIAccount>();
                var list_action_delete = new List<Models.Action>();

                var list_exist_submit = await _repoKPIAc.FindAll(x => x.KpiId == model.Id).ToListAsync();
                if (list_exist_submit.Count > 0)
                {
                    foreach (var acId in model.KpiIds)
                    {
                        var check_exist = _repoKPIAc.FindAll(x => x.AccountId == acId && x.KpiId == item.Id).FirstOrDefault();
                        if (check_exist == null)
                        {
                            var dataAdd = await AddKPIAccount(acId, item.Id);
                            list.Add(dataAdd);
                        }
                        else
                        {
                            if (check_exist.IsActionSubmit)
                            {
                                list_exist.Add(check_exist);
                            }
                            else
                            {
                                list_delete.Add(check_exist);//remove xong add lai

                                var dataAdd = await AddKPIAccount(acId, item.Id);
                                list.Add(dataAdd);

                            }
                        }
                    }
                    foreach (var items in list_exist_submit)
                    {
                        if (model.KpiIds.Contains(items.AccountId))
                        {
                            list_exist.Add(items);
                        }
                        else
                        {
                            list_delete.Add(items);
                            list_account_remove.Add(items);
                        }
                    }

                }
                else
                {
                    foreach (var acId in model.KpiIds)
                    {
                        var dataAdd = await AddKPIAccount(acId, item.Id);
                        list.Add(dataAdd);
                    }
                }
                // xóa PIC thì xóa luôn action của PIC đó
                // foreach (var item_action_delete in list_account_remove)
                // {
                //     var action = _repoAction.FindAll(x => x.KPIId == model.Id && x.AccountId == item_action_delete.AccountId).ToList();
                //     list_action_delete.AddRange(action);
                // }
                _repoKPIAc.RemoveMultiple(list_delete);
                // _repoAction.RemoveMultiple(list_action_delete);
                _repoKPIAc.AddRange(list);
                await _repoKPIAc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public async  Task<KPIAccount> AddKPIAccount(int acId , int kpiId)
        {
            var dataAdd = new KPIAccount
            {
                AccountId = acId,
                KpiId = kpiId,
                DeptId = _repoAc.FindAll(x => x.Id == acId).FirstOrDefault() != null ?
                _repoAc.FindAll(x => x.Id == acId).FirstOrDefault().DeptId : 0,
                CenterId = _repoAc.FindAll(x => x.Id == acId).FirstOrDefault() != null ?
                _repoAc.FindAll(x => x.Id == acId).FirstOrDefault().CenterId : 0,
                FactId = _repoAc.FindAll(x => x.Id == acId).FirstOrDefault() != null ?
                _repoAc.FindAll(x => x.Id == acId).FirstOrDefault().FactId : 0

            };

            return dataAdd;


        }
        public async Task<OperationResult> UpdateSequence(KPINewDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                item.Name = model.Name;
                item.PolicyId = model.PolicyId;
                item.TypeId = model.TypeId;
                item.Sequence = model.Sequence;
                if (model.UpdateBy > 0)
                {
                    item.UpdateBy = model.UpdateBy;
                }
                item.SequenceCHM = model.SequenceCHM;
                item.StartDisplayMeetingTime = Convert.ToDateTime(model.StartDisplayMeetingTime);
                item.EndDisplayMeetingTime = Convert.ToDateTime(model.EndDisplayMeetingTime);
                _repo.Update(item);
                if (model.KpiIds.Count == 0)
                {
                    var removingList = await _repoKPIAc.FindAll(x => x.KpiId == model.Id).ToListAsync();
                    _repoKPIAc.RemoveMultiple(removingList);
                }

                var list = new List<KPIAccount>();
                var list_exist = new List<KPIAccount>();
                var list_delete = new List<KPIAccount>();

                var list_exist_submit = await _repoKPIAc.FindAll(x => x.KpiId == model.Id).ToListAsync();
                if (list_exist_submit.Count > 0)
                {
                    foreach (var acId in model.KpiIds)
                    {
                        var check_exist = _repoKPIAc.FindAll(x => x.AccountId == acId && x.KpiId == item.Id).FirstOrDefault();
                        if (check_exist == null)
                        {
                            var dataAdd = await AddKPIAccount(acId, item.Id);
                            list.Add(dataAdd);
                        }else
                        {
                            if (check_exist.IsActionSubmit)
                            {
                                list_exist.Add(check_exist);
                            }else
                            {
                                list_delete.Add(check_exist);//remove xong add lai

                                var dataAdd = await AddKPIAccount(acId, item.Id);
                                list.Add(dataAdd);

                            }
                        }
                    }
                    foreach (var items in list_exist_submit)
                    {
                        if (model.KpiIds.Contains(items.AccountId))
                        {
                            list_exist.Add(items);
                        }
                        else
                        {
                            list_delete.Add(items);
                        }
                    }

                }
                else
                {
                    foreach (var acId in model.KpiIds)
                    {
                        var dataAdd = await AddKPIAccount(acId, item.Id);
                        list.Add(dataAdd);
                    }
                }
                _repoKPIAc.RemoveMultiple(list_delete);
                _repoKPIAc.AddRange(list);
                await _repoKPIAc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<int> AddKPINew(KPINew item)
        {
            _repo.Add(item);
            await _repo.SaveAll();
            return item.Id;

        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.FindById(id);
            var itemChild = _repo.FindAll(x => x.ParentId == id).ToList();
            var itemSubChild = new List<KPINew>();
            if (itemChild != null)
            {
                foreach (var items in itemChild)
                {
                    var itemSubChilds = _repo.FindAll(x => x.ParentId == items.Id).ToList();
                    if (itemSubChilds != null)
                    {
                        itemSubChild.AddRange(itemSubChilds);
                    }
                }
                _repo.RemoveMultiple(itemSubChild);
                _repo.RemoveMultiple(itemChild);
            }
            
            try
            {
                _repo.Remove(item);
                await _repo.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public async Task<bool> IsDisPlayTodoUpdate(int ID)
        {
            var item = _repo.FindById(ID);
            item.IsDisplayTodo = !item.IsDisplayTodo;
            _repo.Update(item);
            return await _repo.SaveAll();
        }

        public async Task<byte[]> ExportExcelKpiNew(string lang)
        {
            return ExportExcelConsumptionCase1(lang);
            //throw new NotImplementedException();
        }
        private Byte[] ExportExcelConsumptionCase1(string lang)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Huu Quynh";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "KpiNew";



                var lists = new List<KPINewDto>();
                var lists2 = new List<KPINewDto>();
                
                lists = (from x in (_repo.FindAll(x => x.Level == 3).OrderBy(x => x.Name).ToList())
                        join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics
                        let pic = pics.Select(a => a.AccountId).ToList()
                        let mgmt = pics.Select(x =>
                            x.FactId > 0 && x.CenterId > 0 && x.DeptId > 0 ? _repoOc.FindById(x.DeptId).Name
                            : x.FactId > 0 && x.CenterId > 0 && x.DeptId == 0 ? _repoOc.FindById(x.CenterId).Name
                            : x.FactId > 0 && x.CenterId == 0 && x.DeptId == 0 ? _repoOc.FindById(x.FactId).Name : null
                            )
                        let pic_name = pics.Select(x => _repoAc.FindById(x.AccountId).FullName)
                        join t in _repoType.FindAll() on x.TypeId equals t.Id into type_Name
                        let type_name = type_Name.FirstOrDefault()
                        join ac in _repoAc.FindAll() on x.UpdateBy equals ac.Id
                        let name_parent = _repo.FindById(x.ParentId) != null ? _repo.FindById(x.ParentId).Name : "N/A"
                        select new KPINewDto
                        {
                            Id = x.Id,
                            ParentId = x.ParentId,
                            NameParent = name_parent,
                            Name = x.Name,
                            IsDisplayTodo = x.IsDisplayTodo,
                            UpdateBy = x.UpdateBy,
                            Pics = pic.Count > 0 ? pic : new List<int> { },
                            CreateBy = x.CreateBy,
                            LevelOcCreateBy = x.LevelOcCreateBy,
                            OcIdCreateBy = x.OcIdCreateBy,
                            TypeId = x.TypeId,
                            Sequence = x.Sequence,
                            Year = x.Year != null ? x.Year : x.CreatedTime.Year.ToString(),
                            Level = x.Level - 1,
                            StartDisplayMeetingTime = x.StartDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a" ?
                            x.StartDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                            EndDisplayMeetingTime = x.EndDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a"
                            ? x.EndDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                            TypeName = x.TypeId == 0 ? "" : lang == SystemLang.EN ? type_name.NameEn : type_name.NameZh,
                            PICName = pic.Count > 0 ? String.Join(" , ", pic_name) : null,
                            UpdateDate = x.UpdateDate.ToString("MM/dd/yyyy"),
                            CreatedTimeYear = x.CreatedTime.Year.ToString(),
                            UpdateName = ac != null ? ac.FullName : "",
                            Mgmt = String.Join(" , ", mgmt)

                        }).OrderBy(x => x.ParentId).ThenBy(x => x.Sequence).ToList();

                    lists2 = (from x in (_repo.FindAll(x => x.Level == 2).OrderBy(x => x.Name).ToList())
                        join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics
                        let pic = pics.Select(a => a.AccountId).ToList()
                        let mgmt = pics.Select(x =>
                            x.FactId > 0 && x.CenterId > 0 && x.DeptId > 0 ? _repoOc.FindById(x.DeptId).Name
                            : x.FactId > 0 && x.CenterId > 0 && x.DeptId == 0 ? _repoOc.FindById(x.CenterId).Name
                            : x.FactId > 0 && x.CenterId == 0 && x.DeptId == 0 ? _repoOc.FindById(x.FactId).Name : null
                            )
                        let pic_name = pics.Select(x => _repoAc.FindById(x.AccountId).FullName)
                        join t in _repoType.FindAll() on x.TypeId equals t.Id into type_Name
                        let type_name = type_Name.FirstOrDefault()
                        join ac in _repoAc.FindAll() on x.UpdateBy equals ac.Id
                        let name_parent = _repo.FindById(x.ParentId) != null ? _repo.FindById(x.ParentId).Name : "N/A"
                        select new KPINewDto
                        {
                            Id = x.Id,
                            ParentId = x.ParentId,
                            NameParent = name_parent,
                            Name = x.Name,
                            IsDisplayTodo = x.IsDisplayTodo,
                            UpdateBy = x.UpdateBy,
                            Pics = pic.Count > 0 ? pic : new List<int> { },
                            CreateBy = x.CreateBy,
                            LevelOcCreateBy = x.LevelOcCreateBy,
                            OcIdCreateBy = x.OcIdCreateBy,
                            TypeId = x.TypeId,
                            Sequence = x.Sequence,
                            Year = x.Year != null ? x.Year : x.CreatedTime.Year.ToString(),
                            Level = x.Level - 1,
                            StartDisplayMeetingTime = x.StartDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a" ?
                            x.StartDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                            EndDisplayMeetingTime = x.EndDisplayMeetingTime.ToStringDateTime("MM/dd/yyyy") != "n/a"
                            ? x.EndDisplayMeetingTime.ToStringDateTime("MMMM yyyy") : null,
                            TypeName = x.TypeId == 0 ? "" : lang == SystemLang.EN ? type_name.NameEn : type_name.NameZh,
                            PICName = pic.Count > 0 ? String.Join(" , ", pic_name) : null,
                            UpdateDate = x.UpdateDate.ToString("MM/dd/yyyy"),
                            CreatedTimeYear = x.CreatedTime.Year.ToString(),
                            UpdateName = ac != null ? ac.FullName : "",
                            Mgmt = String.Join(" , ", mgmt)

                        }).OrderBy(x => x.ParentId).ThenBy(x => x.Sequence).ToList();                

                    
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("KpiNew");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["KpiNew"];


                    // đặt tên cho sheet
                    ws.Name = "Sheet1";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 12;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";
                    var headers = new string[]{
                        "KPI Name Level 1", "KPI Name",
                        "Type", "PIC", "Mgmt.Arrange",
                        "Start Date", "End Date",  "The last editor", "The last edit date"
                    };

                    int headerRowIndex = 1;
                    int headerColIndex = 1;
                    foreach (var header in headers)
                    {
                        int col = headerRowIndex++;
                        ws.Cells[headerColIndex, col].Value = header;
                        ws.Cells[headerColIndex, col].Style.Font.Bold = true;
                        ws.Cells[headerColIndex, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex = 1;
                    int rowIndex = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in lists)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;


                        //gán giá trị cho từng cell                      
                        ws.Cells[rowIndex, colIndex++].Value = body.NameParent;
                        ws.Cells[rowIndex, colIndex++].Value = body.Name;
                        ws.Cells[rowIndex, colIndex++].Value = body.TypeName;
                        ws.Cells[rowIndex, colIndex++].Value = body.PICName;
                        ws.Cells[rowIndex, colIndex++].Value = body.Mgmt;
                        ws.Cells[rowIndex, colIndex++].Value = body.StartDisplayMeetingTime;
                        ws.Cells[rowIndex, colIndex++].Value = body.StartDisplayMeetingTime;
                        ws.Cells[rowIndex, colIndex++].Value = body.UpdateName;
                        ws.Cells[rowIndex, colIndex++].Value = body.UpdateDate;
                    }

                    // make the borders of cell F6 thick
                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headers.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        if (col == 7 || col == 6)
                        {
                            ws.Column(col).AutoFit(10);
                        }
                        else
                        {
                            ws.Column(col).AutoFit();
                        }
                    }


                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("KpiNew2");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws2 = p.Workbook.Worksheets["KpiNew2"];


                    // đặt tên cho sheet
                    ws2.Name = "Sheet2";
                    // fontsize mặc định cho cả sheet
                    ws2.Cells.Style.Font.Size = 12;
                    // font family mặc định cho cả sheet
                    ws2.Cells.Style.Font.Name = "Calibri";
                    var headers2 = new string[]{
                        "Level", "KPI Name",
                        "Type", "PIC", "Mgmt.Arrange",
                        "Start Date", "End Date",  "The last editor", "The last edit date"
                    };

                    int headerRowIndex2 = 1;
                    int headerColIndex2 = 1;
                    foreach (var header2 in headers2)
                    {
                        int col = headerRowIndex2++;
                        ws2.Cells[headerColIndex2, col].Value = header2;
                        ws2.Cells[headerColIndex2, col].Style.Font.Bold = true;
                        ws2.Cells[headerColIndex2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex2 = 1;
                    int rowIndex2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body2 in lists2)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex2++;


                        //gán giá trị cho từng cell                      
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.Level;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.Name;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.TypeName;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.PICName;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.Mgmt;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.StartDisplayMeetingTime;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.StartDisplayMeetingTime;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.UpdateName;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.UpdateDate;
                    }

                    // make the borders of cell F6 thick
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headers.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        if (col == 7 || col == 6)
                        {
                            ws2.Column(col).AutoFit(10);
                        }
                        else
                        {
                            ws2.Column(col).AutoFit();
                        }
                    }


                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("KpiNew3");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws3 = p.Workbook.Worksheets["KpiNew3"];


                    // đặt tên cho sheet
                    ws3.Name = "Sheet3";
                    // fontsize mặc định cho cả sheet
                    ws3.Cells.Style.Font.Size = 12;
                    // font family mặc định cho cả sheet
                    ws3.Cells.Style.Font.Name = "Calibri";
                    var headers3 = new string[]{
                        "Level", "KPI Name",
                        "Type", "PIC", "Mgmt.Arrange",
                        "Start Date", "End Date",  "The last editor", "The last edit date"
                    };

                    int headerRowIndex3 = 1;
                    int headerColIndex3 = 1;
                    foreach (var header3 in headers3)
                    {
                        int col = headerRowIndex3++;
                        ws3.Cells[headerColIndex3, col].Value = header3;
                        ws3.Cells[headerColIndex3, col].Style.Font.Bold = true;
                        ws3.Cells[headerColIndex3, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex3 = 1;
                    int rowIndex3 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body3 in lists)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex3 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex3++;


                        //gán giá trị cho từng cell                      
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.Level;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.Name;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.TypeName;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.PICName;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.Mgmt;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.StartDisplayMeetingTime;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.StartDisplayMeetingTime;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.UpdateName;
                        ws3.Cells[rowIndex3, colIndex3++].Value = body3.UpdateDate;
                    }

                    // make the borders of cell F6 thick
                    ws3.Cells[ws3.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws3.Cells[ws3.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws3.Cells[ws3.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws3.Cells[ws3.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headers.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws3.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws3.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        if (col == 7 || col == 6)
                        {
                            ws3.Column(col).AutoFit(10);
                        }
                        else
                        {
                            ws3.Column(col).AutoFit();
                        }
                    }
                    
                    
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.Write(mes);
                return new Byte[] { };
            }
        }
    }
}
