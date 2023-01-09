using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
using System.Globalization;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
   
    public class Todolist2Service : IToDoList2Service
    {
        private readonly IActionRepository _repoAction;
        private readonly IDoRepository _repoDo;
        private readonly IKPINewRepository _repoKPINew;
        private readonly ITargetYTDRepository _repoTargetYTD;
        private readonly ITypeRepository _repoType;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IActionStatusRepository _repoActionStatus;
        private readonly ITargetRepository _repoTarget;
        private readonly IStatusRepository _repoStatus;
        private readonly IAccountRepository _repoAc;
        private readonly ITargetPICRepository _repoTargetPIC;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountRepository _repoAccount;
        private readonly IAccountGroupAccountRepository _repoAccountGroupAccount;
        private readonly ISettingMonthRepository _repoSettingMonthly;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;


        public Todolist2Service(
            IActionRepository repoAction,
            IDoRepository repoDo,
            ITypeRepository repoType,
            ITargetPICRepository repoTargetPIC,
            IKPIAccountRepository repoKPIAc,
            IAccountRepository repoAc,
            IKPINewRepository repoKPINew,
            ITargetYTDRepository repoTargetYTD,
            IActionStatusRepository repoActionStatus,
            ITargetRepository repoTarget,
            IStatusRepository repoStatus,
            IHttpContextAccessor httpContextAccessor,
            IAccountRepository repoAccount,
            IAccountGroupAccountRepository repoAccountGroupAccount,
            ISettingMonthRepository repoSettingMonthly, IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repoAction = repoAction;
            _repoTargetPIC = repoTargetPIC;
            _repoDo = repoDo;
            _repoType = repoType;
            _repoAc = repoAc;
            _repoKPINew = repoKPINew;
            _repoTargetYTD = repoTargetYTD;
            _repoKPIAc = repoKPIAc;
            _repoActionStatus = repoActionStatus;
            _repoTarget = repoTarget;
            _repoStatus = repoStatus;
            _httpContextAccessor = httpContextAccessor;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoSettingMonthly = repoSettingMonthly;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<OperationResult> AddOrUpdateStatus(ActionStatusRequestDto request)
        {

            try
            {
                var yearResult = request.CurrentTime.Month == 1 ? request.CurrentTime.Year - 1 : request.CurrentTime.Year;
                var monthResult = request.CurrentTime.Month == 1 ? 12 : request.CurrentTime.Month - 1;
                var updateTime = new DateTime(yearResult, monthResult, 1);
                var result = new ActionStatus();
                if (request.ActionStatusId > 0)
                {

                    var item = await _repoActionStatus.FindAll(x => x.Id == request.ActionStatusId).FirstOrDefaultAsync();
                    item.StatusId = request.StatusId;
                    _repoActionStatus.Update(item);
                    result = item;
                } else
                {
                    var addItem = new ActionStatus
                    {
                        ActionId = request.ActionId,
                        StatusId = request.StatusId,
                        CreatedTime = updateTime,
                        IsDelete = false
                    };
                    _repoActionStatus.Add(addItem);
                    result = addItem;

                }


                await _repoActionStatus.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repoAction.FindById(id);
            try
            {
                _repoAction.Remove(item);
                await _repoAction.SaveAll();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<object> GetActionsForL0(int kpiNewId, int userId)
        {
            var accountId = userId;
            var actions = await _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId).OrderBy(x => x.CreatedTime).ProjectTo<ActionDto>(_configMapper).ToListAsync();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();
            var parentKpi = await _repoKPINew.FindAll(x => x.Id == kpiModel.ParentId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var typeText = _repoType.FindAll(x => x.Id == kpiModel.TypeId).FirstOrDefault() != null ? _repoType.FindAll(x => x.Id == kpiModel.TypeId).FirstOrDefault().Description : null;
            var target = await _repoTarget.FindAll(x => x.KPIId == kpiNewId).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
            var targetYTD = await _repoTargetYTD.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == DateTime.Now.Year).ProjectTo<TargetYTDDto>(_configMapper).FirstOrDefaultAsync();
            var kpi = kpiModel.Name;
            var policy = parentKpi.Name;
            return new
            {
                Actions = actions,
                Kpi = kpi,
                Policy = policy,
                Pic = _repoKPIAc.FindAll(x => x.KpiId == kpiNewId).ToList().Count  > 0 ? String.Join(" , ", kpiModel.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                Target = target,
                typeText = typeText,
                TargetYTD = targetYTD // Target YTD	
            };


        }

        public async Task<object> GetActionsForUpdatePDCA(int kpiNewId, DateTime currentTime, int userId )
        {
            var accountId = userId;
            var nextMonth = currentTime.Month;
            var nextYear = currentTime.Year;
            var actions = await _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == nextYear && x.CreatedTime.Month == nextMonth).ProjectTo<ActionDto>(_configMapper).ToListAsync();
            return new
            {
                Actions = actions,
            };

        }

        public async Task<object> GetKPIForUpdatePDC(int kpiNewId, DateTime currentTime)
        {
            var type = _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefault().TypeId;
            var typeText = _repoType.FindAll(x => x.Id == type).FirstOrDefault().Description;
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();
            var parentKpi = await _repoKPINew.FindAll(x => x.Id == kpiModel.ParentId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policy = parentKpi.Name;
            var kpi = kpiModel.Name;

            return new
            {
                Kpi = kpi,
                Type = type,
                Policy = policy,
                typeText = typeText,
                Pic = _repoKPIAc.FindAll(x => x.KpiId == kpiNewId).ToList().Count > 0 ? String.Join(" , ", kpiModel.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
            };

        }

        public static IEnumerable<(string Month, int Year)> MonthsBetween( DateTime? startDate,DateTime? endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
                limit = endDate.Value;
            }
            else
            {
                iterator = new DateTime(endDate.Value.Year, endDate.Value.Month, 1);
                limit = startDate.Value;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return (
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year
                );

                iterator = iterator.AddMonths(1);
            }
        }

        public async Task<object> GetPDCAForL02(int kpiNewId, DateTime currentTime , int userId)
        {

            var accountId = userId;
            var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
            List<int> listLabelData = new List<int>();
            var month_KPI = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var thisMonthResult_fake = currentTime.Month == month_KPI.Value.Month ? currentTime.Month : currentTime.Month - 1;
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var month = CodeUtility.ConvertNumberMothToString(thisMonthResult_fake);
            var model = new List<UpdatePDCADto>();
            var model_result = new List<UpdatePDCADto>();

            for (int i = 1; i <= thisMonthResult; i++)
            {
                listLabelData.Add(i);
            }
            model = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId 
                        && x.AccountId == accountId 
                        &&  x.CreatedTime.Year == thisYearResult 
                        && x.CreatedTime.Month <= thisMonthResult  )
                        select new UpdatePDCADto
                        {
                            ActionId = a.Id,
                            DoId = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Id : 0,
                            Content = a.Content,
                            DoContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Content : "",
                            ResultContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).ReusltContent : "",
                            Achievement = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Achievement : "",
                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd/yyyy") : "",
                            StatusId = a.ActionStatus.Any(x=> x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).StatusId : Constants.Status.Processing,
                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Id : null,
                            Target = a.Target
                        }).ToList();

            foreach (var item in listLabelData)
            {
                if (item == SystemMonth.Jan)
                {
                    var model2 = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId
                            && x.AccountId == accountId
                            && x.CreatedTime.Year == thisYearResult - 1
                            && x.CreatedTime.Month <= 12)
                            .Where(x =>
                             (x.ActionStatus.FirstOrDefault(c => hideStatus.Contains(c.StatusId)) == null && x.ActionStatus.Count > 0)
                            ||
                            (x.ActionStatus.FirstOrDefault(c => x.CreatedTime.Year <= thisYearResult - 1
                            && x.CreatedTime.Month <= 12
                            && !c.Submitted) != null)
                            || x.ActionStatus.Count == 0
                            )
                            select new UpdatePDCADto
                            {
                                ActionId = a.Id,
                                DoId = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Id : 0,
                                Content = a.Content,
                                DoContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Content : "",
                                ResultContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).ReusltContent : "",
                                Achievement = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Achievement : "",
                                Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).StatusId : Constants.Status.Processing,
                                ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult) ?
                                a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult).Id : null,
                                Target = a.Target
                            }).ToList();
                    model.AddRange(model2);
                }
            }
            var data = model.OrderBy(x => x.ActionId);


            return new
            {
                Data = data,
                Month = month
                //Result = result
            };

        }

        public async Task<object> GetPDCAForL0(int kpiNewId, DateTime currentTime, int userId)
        {

            var accountId = userId;
            var displayStatus2 = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var hideStatus2 = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
            List<int> listLabelData = new List<int>();
            var month_KPI_Start = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var month_KPI_End = _repoKPINew.FindById(kpiNewId).EndDisplayMeetingTime;
            var thisMonthResult_fake = currentTime.Month == month_KPI_Start.Value.Month && currentTime.Year == month_KPI_End.Value.Year ? currentTime.Month : currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var month = CodeUtility.ConvertNumberMothToString(thisMonthResult_fake);
            var model = new List<UpdatePDCADto>();
            var model_result = new List<UpdatePDCADto>();

            var dataTable = new List<DataTablePDCA>();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();

            //var dataStoreProcedure = _repoAction.SqlStoreProcedure(kpiId);
            if (kpiModel.StartDisplayMeetingTime == null && kpiModel.EndDisplayMeetingTime == null)
            {
                return new ChartDtoDateTime
                {
                    Status = false
                };
            }
            var months = MonthsBetween(kpiModel.StartDisplayMeetingTime, kpiModel.EndDisplayMeetingTime);
            foreach (var item in months.Where(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture).Month <= thisMonthResult && x.Year <= currentTime.Year))
            {
                var monthNum = DateTime.ParseExact(item.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                var yearNum = item.Year;
                var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
                var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
                var currentMonthData = new List<UpdatePDCADto>(); // list công việc tháng hiện tại
                var undoneList = new List<UpdatePDCADto>(); // list công việc chưa hoàn thành

                //start
                if (monthNum == SystemMonth.Jan) // nếu là tháng 1 thì tìm list công việc chưa hoàn thành bắt đầu từ tháng 12 trở về trước của năm trước
                {
                    undoneList = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year < yearNum - 1 && x.CreatedTime.Month <= SystemMonth.Dec)
                                  select new UpdatePDCADto
                                  {
                                      ActionId = a.Id,
                                      CreatedBy = a.Account.FullName,
                                      StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).StatusId : 0,
                                      ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).Id : 0,
                                      IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).IsDelete : false
                                  }).Where(y => !hideStatus.Contains((int)y.StatusId) && !y.IsDelete).ToList();
                }
                else
                {
                    //tìm list công việc của năm trước chưa hoàn thành => add vào undoneList
                    var undoneListPreiousYear = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec)
                                                 select new UpdatePDCADto
                                                 {
                                                     ActionId = a.Id,
                                                     CreatedBy = a.Account.FullName,
                                                     StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                                                     ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0,
                                                     IsDelete= a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).IsDelete: false
                                                 }).Where(y => !hideStatus.Contains((int)y.StatusId) && !y.IsDelete).ToList();
                    if (undoneListPreiousYear.Count > 0)
                    {
                        undoneList.AddRange(undoneListPreiousYear);
                    }
                    //end

                    //tìm list công việc của năm hiện tại chưa hoàn thành => add vào undoneList
                    var undoneListCurrentYear =
                    (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month < monthNum)
                     select new UpdatePDCADto
                     {
                         ActionId = a.Id,
                         CreatedBy = a.Account.FullName,
                         StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                         ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0,
                         IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).IsDelete : false
                     }).Where(x => !x.IsDelete).ToList();
                    undoneList.AddRange(undoneListCurrentYear);
                    //end
                }
                //end

                //start
                if (undoneList.Count > 0)
                {
                    //star => tìm list công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                        //join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                        //from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = monthNum.ToString(),
                                            Year = yearNum,
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            //sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Content : null,
                                            //sub == null ? "" : sub.Content,
                                            ResultContent =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).ReusltContent : null,
                                            //sub == null ? "" : sub.ReusltContent,
                                            Achievement =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Achievement : null,
                                            //sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).StatusId : Constants.Status.Processing,

                                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).IsDelete : false,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end

                    //thêm list công việc chưa làm xong của tháng trước vào tháng hiện tại
                    foreach (var itemAcs in undoneList)
                    {
                        var check = _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault() != null ?
                        _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault().StatusId : 0;
                        if (!hideStatus.Contains((int)check) && check != 0)
                        {
                            try
                            {
                                currentMonthData.Add(new UpdatePDCADto
                                {
                                    Month = monthNum.ToString(),
                                    Year = yearNum,
                                    ActionId = itemAcs.ActionId,
                                    CreatedBy = itemAcs.CreatedBy,
                                    Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                                    CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                                    DoId = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 
                                    ? 0 
                                    : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Id,
                                    DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 
                                    ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Content,
                                    ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 
                                    ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().ReusltContent,
                                    Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 
                                    ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Achievement,
                                    Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.HasValue ? _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                                    StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                                    Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                                    StatusId = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                           _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().StatusId : Constants.Status.Processing,
                                    ActionStatusId = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                            _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().Id : 0,
                                    IsDelete = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                            _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().IsDelete : false
                                });
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                    }
                    //end
                }
                else
                {
                    //tìm công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                        //join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                        //from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = monthNum.ToString(),
                                            Year = thisYearResult,
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            //sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Content : null,
                                            //sub == null ? "" : sub.Content,
                                            ResultContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).ReusltContent : null,
                                            //sub == null ? "" : sub.ReusltContent,
                                            Achievement = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Achievement : null,
                                            //sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).StatusId : Constants.Status.Processing,
                                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).IsDelete : false,
                                            //a.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end
                }
                //end


                var dataAdd = new DataTablePDCA()
                {
                    CurrentMonthData = currentMonthData.Where(x => !x.IsDelete).OrderBy(x => x.CreatedTime).ToList(),
                };
                dataTable.Add(dataAdd);
            }
            var data = new List<UpdatePDCADto>();
            foreach (var item in dataTable)
            {
                if (item.CurrentMonthData.Count > 0)
                {
                    foreach (var items in item.CurrentMonthData)
                    {
                        if (items.Month == thisMonthResult.ToString() && items.Year == thisYearResult)
                        {
                            data.Add(items);

                        }
                    }
                }
            }

            return new
            {
                Data = data,
                Month = month
                //Result = result
            };

            

        }

        public async Task<object> GetPDCAForL0T(int kpiNewId, DateTime currentTime , int userId)
        {

            var accountId = userId;
            var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
            List<int> listLabelData = new List<int>();
            var month_KPI = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var thisMonthResult_fake = currentTime.Month == month_KPI.Value.Month ? currentTime.Month : currentTime.Month - 1;
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var month = CodeUtility.ConvertNumberMothToString(thisMonthResult_fake);
            var model = new List<UpdatePDCADto>();
            var model_result = new List<UpdatePDCADto>();
            
            var monthNum = thisMonthResult;
            var yearNum = thisYearResult;
            var currentMonthData = new List<UpdatePDCADto>(); // list công việc tháng hiện tại
            var undoneList = new List<UpdatePDCADto>(); // list công việc chưa hoàn thành


            //di tu bang action status 

            var dataAction = _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId).ToList();
            var dataActionStt = _repoActionStatus.FindAll(x => !x.IsDelete).ToList();
            var dataStt = _repoStatus.FindAll().ToList();
            var dataAc = _repoAccount.FindAll().ToList();

            //data previous month
            var dataResult_previous = (from x in dataAction
                                       join y in dataActionStt on x.Id equals y.ActionId
                             join s in dataStt on x.StatusId equals s.Id
                             join ac in dataAc on x.AccountId equals ac.Id
                             select new
                             {
                                 ActionId = x.Id,
                                 CreatedBy = ac.FullName,
                                 Content = x.Content,
                                 CreatedTime = x.CreatedTime,
                                 ActCreatedTime = y.CreatedTime,
                                 ActionStatusId = y.Id,
                                 StatusId = y.StatusId,
                                 Deadline = x.Deadline.HasValue ? x.Deadline.Value.ToString("MM/dd") : "",
                                 StatusName = s.Name,
                                 Target = x.Target,
                             }).Where(x => x.ActCreatedTime.Month <= monthNum).DistinctBy(o => o.ActionId).ToList();


         
            if (dataResult_previous.Count > 0)
            {
                var item = _repoAction.FindAll(x => x.AccountId == accountId && x.KPIId == kpiNewId && x.CreatedTime.Month == thisMonthResult).ToList();
                foreach (var items in item)
                {
                    currentMonthData.Add(new UpdatePDCADto
                    {
                        Month_PDCA = monthNum,
                        ActionId = items.Id,
                        CreatedBy = items.Account.FullName,
                        Content = items.Content,
                        CreatedTime = items.CreatedTime,
                        DoContent = _repoDo.FindAll().Where(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).ToList().Count == 0
                            ? "" : _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).FirstOrDefault().Content,
                        DoId = _repoDo.FindAll().Where(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).ToList().Count == 0
                            ? 0 : _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).FirstOrDefault().Id,
                        ResultContent = _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).ToList().Count == 0
                            ? "" : _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).FirstOrDefault().ReusltContent,
                        Achievement = _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).ToList().Count == 0
                            ? "" : _repoDo.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult).FirstOrDefault().Achievement,
                        Deadline = _repoAction.FindAll(x => x.Id == items.Id).FirstOrDefault().Deadline.HasValue
                            ? _repoAction.FindAll(x => x.Id == items.Id).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                        StatusName = _repoStatus.FindAll(x => x.Id == items.Id).ToList().Count == 0
                            ? "" : _repoStatus.FindAll(x => x.Id == items.Id).FirstOrDefault().Name.Trim(),
                        Target = _repoAction.FindAll(x => x.Id == items.Id).FirstOrDefault().Target,
                        ActionStatusId = _repoActionStatus.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult && !x.IsDelete).ToList().Count == 0
                            ? 0 : _repoActionStatus.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult && !x.IsDelete).FirstOrDefault().Id,
                        StatusId = _repoActionStatus.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult && !x.IsDelete).ToList().Count == 0
                            ? Constants.Status.Processing : _repoActionStatus.FindAll(x => x.ActionId == items.Id && x.CreatedTime.Month == thisMonthResult && !x.IsDelete).FirstOrDefault().StatusId 
                    });

                }
            }

            // tim list cong viec chua lam xong cua thang truoc vao thang hien tai

            foreach (var item in dataResult_previous)
            {
                //neu status cuoi cung la complete and terminate thi bo qua
                var itemAcs = _repoActionStatus.FindAll(x => x.ActionId == item.ActionId && !x.IsDelete).OrderBy(x => x.CreatedTime).LastOrDefault();
                //nguoc lai add vao list currentMonthData
                if (!hideStatus.Contains((int)itemAcs.StatusId) || !itemAcs.Submitted )
                {
                    currentMonthData.Add(new UpdatePDCADto
                    {
                        Month_PDCA = monthNum,
                        ActionId = itemAcs.ActionId,
                        ActCreatedTime = itemAcs.CreatedTime,
                        CreatedBy = item.CreatedBy,
                        Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                        CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                        DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 
                        ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Content,
                        DoId = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 
                        ? 0 : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Id,
                        ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 
                        ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().ReusltContent,
                        Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 
                        ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Achievement,
                        Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.HasValue 
                        ? _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                        StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 
                        ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                        Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                        ActionStatusId = itemAcs.Id,
                        StatusId = itemAcs.StatusId,
                    });
                }


            }
            //tim list acction neu chua co status
            var actionCurrent = dataAction.Where(x => x.CreatedTime.Month == thisMonthResult).ToList();
            foreach (var item_current  in actionCurrent)
            {
                var checkExistStatusId = _repoActionStatus.FindAll(x => !x.IsDelete).Any(x => x.ActionId == item_current.Id && x.CreatedTime.Month == item_current.CreatedTime.Month);
                if (!checkExistStatusId)
                {
                    currentMonthData.Add(new UpdatePDCADto
                    {
                        Month_PDCA = monthNum,
                        ActionId = item_current.Id,
                        ActCreatedTime = item_current.CreatedTime,
                        CreatedBy = item_current.Account.FullName,
                        Content = item_current.Content,
                        CreatedTime = item_current.CreatedTime,
                        DoContent = _repoDo.FindAll().Where(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).ToList().Count == 0
                        ? "" : _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).FirstOrDefault().Content,
                            DoId = _repoDo.FindAll().Where(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).ToList().Count == 0
                        ? 0 : _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).FirstOrDefault().Id,
                            ResultContent = _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).ToList().Count == 0
                        ? "" : _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).FirstOrDefault().ReusltContent,
                            Achievement = _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).ToList().Count == 0
                        ? "" : _repoDo.FindAll(x => x.ActionId == item_current.Id && x.CreatedTime.Month == monthNum).FirstOrDefault().Achievement,
                            Deadline = _repoAction.FindAll(x => x.Id == item_current.Id).FirstOrDefault().Deadline.HasValue
                        ? _repoAction.FindAll(x => x.Id == item_current.Id).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                            StatusName = _repoStatus.FindAll(x => x.Id == item_current.Id).ToList().Count == 0
                        ? "" : _repoStatus.FindAll(x => x.Id == item_current.Id).FirstOrDefault().Name.Trim(),
                        Target = _repoAction.FindAll(x => x.Id == item_current.Id).FirstOrDefault().Target,
                        ActionStatusId = 0,
                        StatusId = Constants.Status.Processing,
                    });
                }
            }





            var data = new List<UpdatePDCADto>();
            data = currentMonthData.DistinctBy(x => x.ActionId).ToList();
            //if (currentTime.Month == thisMonthResult_fake)
            //{
            //    data = currentMonthData.Where(x => x.StatusId != 0).OrderBy(x => x.Month_PDCA == thisMonthResult).ToList();
            //}else
            //{
            //    data = currentMonthData.OrderBy(x => x.Month_PDCA == thisMonthResult).ToList();
            //}

            //data.ForEach(item =>
            //{
            //    if (item.StatusId == 0)
            //    {
            //        item.StatusId = Constants.Status.Processing;
            //    }
               
            //});
            return new
            {
                Data = data,
                Month = month
                //Result = result
            };

        }

        public async Task<object> GetPDCAForL0Revise(int kpiNewId, DateTime currentTime, int userId)
        {

            var accountId = userId;
            var displayStatus2 = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var hideStatus2 = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
            List<int> listLabelData = new List<int>();
            var month_KPI_Start = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var month_KPI = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var month_KPI_End = _repoKPINew.FindById(kpiNewId).EndDisplayMeetingTime;
            var thisMonthResult_fake = (currentTime.Month == month_KPI_Start.Value.Month && currentTime.Year == month_KPI_Start.Value.Year) ? currentTime.Month : currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            //var thisMonthResult_fake = currentTime.Month == month_KPI.Value.Month ? currentTime.Month : currentTime.Month - 1;
            var thisMonthResult = (currentTime.Month == month_KPI_Start.Value.Month && currentTime.Year == month_KPI_Start.Value.Year) ? currentTime.Month : currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            //var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var month = CodeUtility.ConvertNumberMothToString(thisMonthResult_fake);
            var model = new List<UpdatePDCADto>();
            var model_result = new List<UpdatePDCADto>();

            var dataTable = new List<DataTablePDCA>();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();

            //var dataStoreProcedure = _repoAction.SqlStoreProcedure(kpiId);
            if (kpiModel.StartDisplayMeetingTime == null && kpiModel.EndDisplayMeetingTime == null)
            {
                return new ChartDtoDateTime
                {
                    Status = false
                };
            }
            var months = MonthsBetween(kpiModel.StartDisplayMeetingTime, kpiModel.EndDisplayMeetingTime);
            var data = new List<UpdatePDCADto>();
            foreach (var item in months.Where(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture).Month <= thisMonthResult && x.Year <= currentTime.Year))
            {
                var monthNum = DateTime.ParseExact(item.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                var yearNum = item.Year;
                var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
                var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
                var currentMonthData = new List<UpdatePDCADto>(); // list công việc tháng hiện tại
                var undoneList = new List<UpdatePDCADto>(); // list công việc chưa hoàn thành

                //start
                if (monthNum == SystemMonth.Jan) // nếu là tháng 1 thì tìm list công việc chưa hoàn thành bắt đầu từ tháng 12 trở về trước của năm trước
                {
                    undoneList = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year < yearNum - 1 && x.CreatedTime.Month <= SystemMonth.Dec)
                                  select new UpdatePDCADto
                                  {
                                      ActionId = a.Id,
                                      CreatedBy = a.Account.FullName,
                                      StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).StatusId : 0,
                                      ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).Id : 0,
                                      IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).IsDelete : false
                                  }).Where(y => !hideStatus.Contains((int)y.StatusId) && !y.IsDelete).ToList();
                }
                else
                {
                    //tìm list công việc của năm trước chưa hoàn thành => add vào undoneList
                    var undoneListPreiousYear = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec)
                                                 select new UpdatePDCADto
                                                 {
                                                     ActionId = a.Id,
                                                     CreatedBy = a.Account.FullName,
                                                     StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                                                     ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0,
                                                     IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).IsDelete : false
                                                 }).Where(y => !hideStatus.Contains((int)y.StatusId) && !y.IsDelete).ToList();
                    if (undoneListPreiousYear.Count > 0)
                    {
                        undoneList.AddRange(undoneListPreiousYear);
                    }
                    //end

                    //tìm list công việc của năm hiện tại chưa hoàn thành => add vào undoneList
                    var undoneListCurrentYear =
                    (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month < monthNum)
                     select new UpdatePDCADto
                     {
                         ActionId = a.Id,
                         CreatedBy = a.Account.FullName,
                         StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                         ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0,
                         IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).IsDelete : false
                     }).Where(x => !x.IsDelete).ToList();
                    undoneList.AddRange(undoneListCurrentYear);
                    //end
                }
                //end

                //start
                if (undoneList.Count > 0)
                {
                    //star => tìm list công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                            //join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                            //from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = monthNum.ToString(),
                                            Year = yearNum,
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            //sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Content : null,
                                            //sub == null ? "" : sub.Content,
                                            ResultContent =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).ReusltContent : null,
                                            //sub == null ? "" : sub.ReusltContent,
                                            Achievement =
                                            a.Does.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Achievement : null,
                                            //sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).StatusId : Constants.Status.Processing,

                                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).IsDelete : false,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end

                    //thêm list công việc chưa làm xong của tháng trước vào tháng hiện tại
                    foreach (var itemAcs in undoneList)
                    {
                        var check = _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault() != null ?
                        _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault().StatusId : 0;
                        if (!hideStatus.Contains((int)check) && check != 0)
                        {
                            try
                            {
                                currentMonthData.Add(new UpdatePDCADto
                                {
                                    Month = monthNum.ToString(),
                                    ActionId = itemAcs.ActionId,
                                    Year = yearNum,
                                    CreatedBy = itemAcs.CreatedBy,
                                    Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                                    CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                                    DoId = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0
                                    ? 0
                                    : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Id,
                                    DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 ? "" 
                                    : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Content,
                                    ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 ? "" 
                                    : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().ReusltContent,
                                    Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).ToList().Count == 0 ? "" 
                                    : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum && x.CreatedTime.Year == yearNum).FirstOrDefault().Achievement,
                                    Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.HasValue ? _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                                    StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                                    Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                                    StatusId = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                           _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().StatusId : Constants.Status.Processing,
                                    ActionStatusId = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                            _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().Id : 0,
                                    IsDelete = _repoActionStatus.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                            _repoActionStatus.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().IsDelete : false,
                                });
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                    }
                    //end
                }
                else
                {
                    if ((month_KPI_Start.Value.Month == currentTime.Month && month_KPI_Start.Value.Year == currentTime.Year) && month_KPI_Start.Value.Year == currentTime.Year)
                    {
                        return new
                        {
                            Data = data,
                            Month = month
                            //Result = result
                        };
                    }
                    //tìm công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.AccountId == accountId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                            //join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                            //from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = monthNum.ToString(),
                                            Year = thisYearResult,
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            //sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Content : null,
                                            //sub == null ? "" : sub.Content,
                                            ResultContent = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).ReusltContent : null,
                                            //sub == null ? "" : sub.ReusltContent,
                                            Achievement = a.Does.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum) ?
                                            a.Does.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Achievement : null,
                                            //sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).StatusId : Constants.Status.Processing,
                                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).Id : 0,
                                            IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).IsDelete : false,
                                            //a.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end
                }
                //end


                var dataAdd = new DataTablePDCA()
                {
                    CurrentMonthData = currentMonthData.Where(x => !x.IsDelete).OrderBy(x => x.CreatedTime).ToList(),
                };
                dataTable.Add(dataAdd);
            }
            
            foreach (var item in dataTable)
            {
                if (item.CurrentMonthData.Count > 0)
                {
                    foreach (var items in item.CurrentMonthData)
                    {
                        if (items.Month == thisMonthResult.ToString() && items.Year == thisYearResult)
                        {
                            data.Add(items);

                        }
                    }
                }
            }

            return new
            {
                Data = data,
                Month = month
                //Result = result
            };

        }


        public async Task<object> GetPDCAForL0ReviseT(int kpiNewId, DateTime currentTime, int userId)
        {

            var accountId = userId;
            var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
            List<int> listLabelData = new List<int>();
            var thisMonthResult = currentTime.Month;
            var thisMonthResult_Fake = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Year == 1 ? currentTime.Year - 1 : currentTime.Year;
            var month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(thisMonthResult_Fake);
            var model = new List<UpdatePDCADto>();
            var model_result = new List<UpdatePDCADto>();

            //for (int i = 1; i <= thisMonthResult_Fake; i++)
            //{
            //    listLabelData.Add(i);
            //}


            var monthNum = thisMonthResult_Fake;
            var yearNum = thisYearResult;
            var currentMonthData = new List<UpdatePDCADto>(); // list công việc tháng hiện tại
            var undoneList = new List<UpdatePDCADto>(); // list công việc chưa hoàn thành
            //start
            if (monthNum == SystemMonth.Jan) // nếu là tháng 1 thì tìm list công việc chưa hoàn thành bắt đầu từ tháng 12 trở về trước của năm trước
            {
                undoneList = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year < yearNum - 1 && x.CreatedTime.Month <= SystemMonth.Dec)
                                select new UpdatePDCADto
                                {
                                    ActionId = a.Id,
                                    CreatedBy = a.Account.FullName,
                                    StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).StatusId : null,
                                    ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec) ?
                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec).Id : null
                                }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
            }
            else
            {
                //tìm list công việc của năm trước chưa hoàn thành => add vào undoneList
                var undoneListPreiousYear = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec)
                                                select new UpdatePDCADto
                                                {
                                                    ActionId = a.Id,
                                                    CreatedBy = a.Account.FullName,
                                                    StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : null,
                                                    ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : null
                                                }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
                if (undoneListPreiousYear.Count > 0)
                {
                    undoneList.AddRange(undoneListPreiousYear);
                }
                //end

                //tìm list công việc của năm hiện tại chưa hoàn thành => add vào undoneList
                var undoneListCurrentYear =
                (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month < monthNum)
                    select new UpdatePDCADto
                    {
                        ActionId = a.Id,
                        CreatedBy = a.Account.FullName,
                        StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                        ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                    a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0
                    }).ToList();
                undoneList.AddRange(undoneListCurrentYear);
                //end
            }
            //end

            //start
            if (undoneList.Count > 0)
            {
                //star => tìm list công việc tháng hiện tại
                currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                    join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                    from sub in ab.DefaultIfEmpty()
                                    select new UpdatePDCADto
                                    {
                                        Month_PDCA = monthNum,
                                        ActionId = a.Id,
                                        CreatedBy = a.Account.FullName,
                                        DoId = sub == null ? 0 : sub.Id,
                                        Content = a.Content,
                                        CreatedTime = a.CreatedTime,
                                        DoContent = sub == null ? "" : sub.Content,
                                        ResultContent = sub == null ? "" : sub.ReusltContent,
                                        Achievement = sub == null ? "" : sub.Achievement,
                                        ActionStatusId = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == monthNum).Id,
                                        StatusId = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == monthNum).StatusId,
                                        Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                        StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum).Status.Name.Trim(),
                                        Target = a.Target,
                                    }).ToList();
                //end

                //thêm list công việc chưa làm xong của tháng trước vào tháng hiện tại
                foreach (var itemAcs in undoneList)
                {
                    var check = _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1).FirstOrDefault() != null ?
                    _repoActionStatus.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1).FirstOrDefault().StatusId : 0;
                    if (!hideStatus.Contains((int)check) && check != 0)
                    {
                        try
                        {
                            currentMonthData.Add(new UpdatePDCADto
                            {
                                Month_PDCA = monthNum,
                                ActionId = itemAcs.ActionId,
                                CreatedBy = itemAcs.CreatedBy,
                                Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                                CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                                DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Content,
                                DoId = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? 0 : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Id,
                                ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().ReusltContent,
                                Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Achievement,
                                Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.HasValue ? _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                                StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                                Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                                ActionStatusId = itemAcs.ActionStatusId,
                                StatusId = itemAcs.StatusId
                            });
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
                //end
            }
            else
            {
                //tìm công việc tháng hiện tại
                currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                    join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                    from sub in ab.DefaultIfEmpty()
                                    select new UpdatePDCADto
                                    {
                                        Month_PDCA = monthNum,
                                        ActionId = a.Id,
                                        CreatedBy = a.Account.FullName,
                                        DoId = sub == null ? 0 : sub.Id,
                                        CreatedTime = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).CreatedTime,
                                        Content = a.Content,
                                        DoContent = sub == null ? "" : sub.Content,
                                        ResultContent = sub == null ? "" : sub.ReusltContent,
                                        Achievement = sub == null ? "" : sub.Achievement,
                                        Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                        StatusId = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).StatusId,
                                        ActionStatusId = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).Id,
                                        StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).Status.Name.Trim(),
                                        Target = a.Target,
                                    }).ToList();
                //end
            }
            //end
          

            var data = currentMonthData.OrderBy(x => x.Month_PDCA == thisMonthResult_Fake);

            return new
            {
                Data = data,
                Month = month
                //Result = result
            };

        }

        public async Task<object> GetStatus()
        {
            return await _repoStatus.FindAll().ToListAsync();

        }

        public async Task<object> GetTargetForUpdatePDCA(int kpiNewId, DateTime currentTime)
        {
            var nextYear = currentTime.Month == 12 ? currentTime.Year + 1 : currentTime.Year;
            var month_KPI = _repoKPINew.FindById(kpiNewId).StartDisplayMeetingTime;
            var thisMonth = currentTime.Month == month_KPI.Value.Month ? currentTime.Month : currentTime.Month - 1;
            var nextMonth = currentTime.Month;
            var thisYear = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var nextMonthTarget = await _repoTarget.FindAll(x => x.KPIId == kpiNewId && x.TargetTime.Year == currentTime.Year && x.TargetTime.Month == nextMonth).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
            var target = await _repoTarget.FindAll(x => x.KPIId == kpiNewId && x.TargetTime.Year == currentTime.Year && x.TargetTime.Month == thisMonth).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
            var targetYTD = await _repoTargetYTD.FindAll(x => x.KPIId == kpiNewId).ProjectTo<TargetYTDDto>(_configMapper).FirstOrDefaultAsync();

            return new
            {
                ThisMonthYTD = target,
                ThisMonthPerformance = target,
                ThisMonthTarget = target,
                TargetYTD = targetYTD,
                NextMonthTarget = nextMonthTarget,
                Month = month_KPI.Value.Month
            };

        }

        public async Task<object> L0(DateTime currentTime, int userId)
        {
            int accountId = userId;
            var account = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (account == null) return null;

            var date = currentTime;
            var month = date.Month;
            List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
            var month2 = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var year = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            //Display update PDCA
            var PDCA = await _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => new
            {
                Id = x.KpiId,
                Topic = x.KPINew.Name,
                Level = x.KPINew.Level,
                PICName = x.KPINew.KPIAccounts.Count > 0 ? String.Join(" , ", x.KPINew.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                TypeText = _repoType.FindById(x.KPINew.TypeId).Description,
                Type = "UpdatePDCA",
                Start = x.KPINew.StartDisplayMeetingTime,
                End = x.KPINew.EndDisplayMeetingTime,
                StartYear = x.KPINew.StartDisplayMeetingTime.Value.Year,
                EndYear = x.KPINew.EndDisplayMeetingTime.Value.Year,
                Year = x.KPINew.Year == null ? x.KPINew.CreatedTime.Year.ToString() : x.KPINew.Year,
                Status = x.KPINew.IsDisplayTodo,
                CurrentTarget = _repoTargetPIC.FindAll().Any(y => y.targetId == x.KPINew.Targets.FirstOrDefault(a => a.TargetTime.Year == currentTime.Year
                && a.TargetTime.Month == currentTime.Month).Id && y.AccountId == accountId && y.IsSubmit) ? false : true
            })
            .Where(x => x.CurrentTarget && x.Level != Level.Level_1).ToListAsync();
            //.Where(x =>  x.Level != Level.Level_1).ToListAsync();
            var PDCA_Result = PDCA.Select(x => new
            {
                x.Id,
                x.Topic,
                x.Level,
                x.PICName,
                x.TypeText,
                x.Type,
                x.Start,
                x.End,
                x.StartYear,
                x.EndYear,
                x.Year,
                x.Status,
                x.CurrentTarget,
                DisplayInCurrentMonth = CheckDisplayInCurrentMonth(x.Id, currentTime),

            }).ToList();
            return PDCA_Result.Where(
                x => x.EndYear <= currentTime.Year
                && x.Status == false 
                && x.DisplayInCurrentMonth
            );
        }

        public bool CheckDisplayInCurrentMonth(int kpiID, DateTime currentTime)
        {
            //var item = _repoKPINew.FindById(kpiID);
            var isBool = false;
            var month_KPI_Start = _repoKPINew.FindById(kpiID).StartDisplayMeetingTime;
            var month_KPI_End = _repoKPINew.FindById(kpiID).EndDisplayMeetingTime;
            var months = MonthsBetween(month_KPI_Start, month_KPI_End);
            var month = currentTime.Month == 1 && month_KPI_End.Value.Month == 12 && month_KPI_End.Value.Year < currentTime.Year ? 12 
                : currentTime.Month == 1  && month_KPI_End.Value.Year == currentTime.Year ? currentTime.Month :
                currentTime.Month == 1 && month_KPI_Start.Value.Year == currentTime.Year ? currentTime.Month :
                currentTime.Month - 1 ;
            var year = currentTime.Month == 1 && month_KPI_End.Value.Month == 12 && month_KPI_End.Value.Year < currentTime.Year ? currentTime.Year - 1 : currentTime.Year; 
            foreach (var x in months)
            {
                if (DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture).Month == month && x.Year == year)
                {
                    isBool = true;
                }
            }
            return isBool;
           
        }


        public async Task<object> L0Revise(DateTime currentTime, int userId)
        {
            var ct = currentTime;
            int accountId = userId;
            var account = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (account == null) return null;

            var date = currentTime;
            var month = date.Month;
            List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
           
            var latestMonth = ct.Month - 1;
            var month2 = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var year = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            //Display update PDCA
            var PDCA = await _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => new
            {
                Id = x.KpiId,
                Topic = x.KPINew.Name,
                Level = x.KPINew.Level,
                PICName = x.KPINew.KPIAccounts.Count > 0 ? String.Join(" , ", x.KPINew.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                TypeText = _repoType.FindById(x.KPINew.TypeId).Description,
                Type = "UpdatePDCA",
                Start = x.KPINew.StartDisplayMeetingTime,
                End = x.KPINew.EndDisplayMeetingTime,
                StartYear = x.KPINew.StartDisplayMeetingTime.Value.Year,
                EndYear = x.KPINew.EndDisplayMeetingTime.Value.Year,
                Year = x.KPINew.Year == null ? x.KPINew.CreatedTime.Year.ToString() : x.KPINew.Year,
                Status = x.KPINew.IsDisplayTodo,
                CurrentTarget = _repoTargetPIC.FindAll().Any(y => y.targetId == x.KPINew.Targets.FirstOrDefault(a => a.TargetTime.Year == currentTime.Year
                && a.TargetTime.Month == currentTime.Month).Id && y.AccountId == accountId && y.IsSubmit) ? false : true
            }).Where(x => x.Level != Level.Level_1).ToListAsync();

            var PDCA_Result = PDCA.Select(x => new
            {
                x.Id,
                x.Topic,
                x.Level,
                x.PICName,
                x.TypeText,
                x.Type,
                x.Start,
                x.End,
                x.StartYear,
                x.EndYear,
                x.Year,
                x.Status,
                x.CurrentTarget,
                DisplayInCurrentMonth = CheckDisplayInCurrentMonth(x.Id, currentTime),

            }).ToList();
            //return PDCA.Where(
            //      x => x.Year.ToInt() <= currentTime.Year
            //      && x.Status == false
            //      && x.Start.Value.Month <= month
            //      //&& x.End.Value.Month >= (month == 1 ? 12 : currentTime.Month - 1)
            //      );

            return PDCA_Result.Where(x =>
                x.Status == false
                && x.DisplayInCurrentMonth
            );
            //return PDCA.Where(x => x.Year == currentTime.Year.ToString() && x.Status == false && x.Status == false && x.Start.Value.Month <= month && x.End.Value.Month >= month);
        }


        public async Task<OperationResult> SubmitAction(ActionRequestDto model)
        {
            int accountId = model.UserId;
            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();
            try
            {
                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var currentTime = model.CurrentTime;
                var kpiId = target.KPIId;
                if (kpiId > 0)
                {
                   var update =  _repoKPIAc.FindAll(x => x.AccountId == accountId && x.KpiId == kpiId).FirstOrDefault();
                    update.IsActionSubmit = true;
                    _repoKPIAc.Update(update);

                }
                
                if (target != null)
                {
                    target.TargetTime = currentTime;

                }

                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);

                int id = 0;
                var targetExist = _repoTarget.FindAll(x =>x.KPIId == kpiId).FirstOrDefault();

                if (targetExist != null)
                {
                    id = targetExist.Id;
                    targetExist.Value = target.Value;
                    _repoTarget.Update(targetExist);
                }
                else
                {
                    target.Submitted = false;
                    id =  await AddTarget(target);
                }
                var targetYTDExist = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).FirstOrDefault();
                if (targetYTDExist != null)
                {
                    targetYTDExist.Value = targetYTD.Value;
                    _repoTargetYTD.Update(targetYTDExist);
                }
                else
                {
                    _repoTargetYTD.Add(targetYTD);
                }
                   
                var targetPic = new TargetPIC()
                {
                    targetId = id,
                    AccountId = accountId,
                    IsSubmit = true
                };

                if (targetPic != null)
                {
                    _repoTargetPIC.Add(targetPic);
                }
                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);

                await _repoAction.SaveAll();
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

        public async Task<int> AddTarget(Target item)
        {
            _repoTarget.Add(item);
            await _repoTarget.SaveAll();
            return item.Id;
        }

        public async Task<int> UpdateTarget(Target item)
        {
            _repoTarget.Update(item);
            await _repoTarget.SaveAll();
            return item.Id;
        }

        public async Task<OperationResult> SaveAction(ActionRequestDto model)
        {
            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();

            try
            {
                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var currentTime = model.CurrentTime;
                if (target != null)
                {
                    target.TargetTime = currentTime;

                }
                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);

                var addActions = _mapper.Map<List<Models.Action>>(addActionList);

                var targetExist = _repoTarget.FindAll(x => x.KPIId == target.KPIId).FirstOrDefault();
                if (targetExist != null)
                {
                     targetExist.Value = target.Value;
                    _repoTarget.Update(targetExist);
                }
                else
                {
                    target.Submitted = false;
                    _repoTarget.Add(target);
                }
                var targetYTDExist = _repoTargetYTD.FindAll(x => x.KPIId == target.KPIId).FirstOrDefault();
                if (targetYTDExist != null)
                {
                    targetYTDExist.Value = targetYTD.Value;
                    _repoTargetYTD.Update(targetYTDExist);
                } else
                {
                    _repoTargetYTD.Add(targetYTD);
                }
                    

                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);

                await _repoAction.SaveAll();
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

        public async Task<OperationResult> SubmitKPINew(int kpiNewId)
        {
            try
            {
                var item = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();
                item.Submitted = true;
                _repoKPINew.Update(item);

                await _repoKPINew.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> SubmitUpdatePDCA(PDCARequestDto model)
        {
            int accountId = model.UserId;
            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();
            var updateActionForThisMonth = model.UpdatePDCA.ToList();
            //var updateActionStatus = model.UpdatePDCA.Where(x => x.ActionStatusId.Value > 0).Select(x=> x.ActionStatusId).ToList();
            try
            {
                var currentTime = model.CurrentTime;

                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var nextMonthTarget = _mapper.Map<Target>(model.NextMonthTarget);
                if (nextMonthTarget != null)
                {
                    nextMonthTarget.TargetTime = currentTime;

                }
                var kpiId = target.KPIId;
                if (kpiId > 0)
                {
                    var updated = _repoKPIAc.FindAll(x => x.AccountId == accountId && x.KpiId == kpiId).FirstOrDefault();
                    updated.IsPDCASubmit = true;
                    _repoKPIAc.Update(updated);
                }

                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);
                //_repoTarget.Update(target);
                //await UpdateTarget(target);
                //await _repoTarget.SaveAll();
                var targetExist = _repoTarget.FindAll(x => x.TargetTime.Year == model.CurrentTime.Year && x.TargetTime.Month == model.CurrentTime.Month && x.KPIId == kpiId).FirstOrDefault();
                int id = 0;

                if (targetExist != null)
                {
                    //_repoTarget.Update(nextMonthTarget);
                    id = targetExist.Id;
                }
                else
                {
                    
                    nextMonthTarget.Submitted = false;
                    nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month, 1);
                    nextMonthTarget.Value = nextMonthTarget.Value;
                    nextMonthTarget.Performance = target.Performance;
                    nextMonthTarget.YTD = target.YTD;
                    id = await AddTarget(nextMonthTarget);
                }
                

                var targetPic = new TargetPIC()
                {
                    targetId = id,
                    AccountId = accountId,
                    IsSubmit = true
                };

                if (targetPic != null)
                {
                    _repoTargetPIC.Add(targetPic);
                }
                
                _repoTargetYTD.Update(targetYTD);
                // dynamic currentime
                addActions.ForEach(item =>
                {
                    item.CreatedTime = model.CurrentTime;
                });

                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);
                var updatethisMonthAction = new List<Models.Action>();
                var addDoList = new List<Do>();
                var updatedoList = new List<Do>();
                foreach (var item in updateActionForThisMonth)
                {
                    var action = await _repoAction.FindAll(x => x.Id == item.ActionId).FirstOrDefaultAsync();
                    action.StatusId = item.StatusId;
                    updatethisMonthAction.Add(action);
                    var yearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
                    var monthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
                    var updateTime = new DateTime(yearResult, monthResult, 1);
                    if (item.DoId == 0)
                    {
                        var addDoItem = new Do(item.DoContent,item.ResultContent, item.Achievement, item.ActionId);
                        addDoItem.CreatedTime = updateTime;
                        addDoList.Add(addDoItem);
                    }
                    else
                    {
                        var doItem = await _repoDo.FindAll(x => x.Id == item.DoId).FirstOrDefaultAsync();
                        doItem.Content = item.DoContent;
                        doItem.Achievement = item.Achievement;
                        doItem.ReusltContent = item.ResultContent;
                        updatedoList.Add(doItem);
                    }

                    var ac_status = await _repoActionStatus.FindAll(x => x.ActionId == item.ActionId && x.CreatedTime.Month == monthResult && x.CreatedTime.Year == yearResult).FirstOrDefaultAsync();
                    if (ac_status == null)
                    {
                        var addItem = new ActionStatus
                        {
                            ActionId = item.ActionId,
                            StatusId = item.StatusId.ToInt(),
                            CreatedTime = updateTime,
                            Submitted = true
                        };
                        _repoActionStatus.Add(addItem);
                    } else
                    {
                        ac_status.Submitted = true;
                        _repoActionStatus.Update(ac_status);
                    }

                }
                //var update = await _repoActionStatus.FindAll(x => updateActionStatus.Contains(x.Id)).ToListAsync();
                //update.ForEach(item =>
                //{
                //    item.Submitted = model.Target.Submitted;
                //});
                //_repoActionStatus.UpdateRange(update);

                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updatethisMonthAction);
                _repoDo.AddRange(addDoList);
                _repoDo.UpdateRange(updatedoList);

                await _repoAc.SaveAll();

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

        public async Task<OperationResult> SaveUpdatePDCA(PDCARequestDto model)
        {

            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();
            var updateActionForThisMonth = model.UpdatePDCA.ToList();
            //var updateActionStatus = model.UpdatePDCA.Where(x => x.ActionStatusId.Value > 0).Select(x => x.ActionStatusId).ToList();
            try
            {
                var currentTime = model.CurrentTime;
                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var nextMonthTarget = _mapper.Map<Target>(model.NextMonthTarget);

                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);
                var month_KPI = _repoKPINew.FindById(nextMonthTarget.KPIId).StartDisplayMeetingTime;
                var kpi_id = nextMonthTarget.KPIId;
                if (target != null && currentTime.Month == month_KPI.Value.Month)
                {
                    target.TargetTime = new DateTime(currentTime.Year, currentTime.Month, 1);

                } else
                {
                    target.TargetTime = new DateTime(currentTime.Year, currentTime.Month - 1, 1);
                }

                if (nextMonthTarget != null)
                {
                    nextMonthTarget.TargetTime = currentTime;

                }

                if (currentTime.Month == month_KPI.Value.Month)
                {
                    var checkExist_current = _repoTarget.FindAll(x =>
                    x.TargetTime.Year == nextMonthTarget.TargetTime.Year
                    && x.TargetTime.Month == nextMonthTarget.TargetTime.Month
                    && x.KPIId == kpi_id).FirstOrDefault();

                    var checkExist_next = _repoTarget.FindAll(x =>
                    x.TargetTime.Year == nextMonthTarget.TargetTime.Year
                    && x.TargetTime.Month == nextMonthTarget.TargetTime.Month + 1
                    && x.KPIId == kpi_id).FirstOrDefault();

                    if (checkExist_current == null)
                    {
                        nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month, 1);
                        nextMonthTarget.Id = 0;
                        nextMonthTarget.Value = nextMonthTarget.Value;
                        nextMonthTarget.Performance = target.Performance;
                        nextMonthTarget.YTD = target.YTD;
                        await AddTarget(nextMonthTarget);
                    } else
                    {
                        checkExist_current.Value = nextMonthTarget.Value;
                        checkExist_current.Performance = target.Performance;
                        checkExist_current.YTD = target.YTD;
                        await UpdateTarget(checkExist_current);
                    }

                    if (checkExist_next == null)
                    {
                        nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month + 1, 1);
                        nextMonthTarget.Id = 0;
                        nextMonthTarget.Value = nextMonthTarget.Value;
                        nextMonthTarget.Performance = 0;
                        nextMonthTarget.YTD = 0;
                        await AddTarget(nextMonthTarget);
                    } else
                    {
                        checkExist_next.Value = nextMonthTarget.Value;
                        //checkExist_next.Performance = target.Performance;
                        //checkExist_next.YTD = target.YTD;
                        //_repoTarget.Update(checkExist_next);
                        await UpdateTarget(checkExist_next);
                    }
                } else
                {

                    var checkExist_current = _repoTarget.FindAll(x =>
                    x.TargetTime.Year == nextMonthTarget.TargetTime.Year
                    && x.TargetTime.Month == nextMonthTarget.TargetTime.Month - 1
                    && x.KPIId == kpi_id).FirstOrDefault();

                    var checkExist_next = _repoTarget.FindAll(x =>
                    x.TargetTime.Year == nextMonthTarget.TargetTime.Year
                    && x.TargetTime.Month == nextMonthTarget.TargetTime.Month
                    && x.KPIId == kpi_id).FirstOrDefault();


                    if (checkExist_current == null)
                    {
                        nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month - 1, 1);
                        nextMonthTarget.Id = 0;
                        nextMonthTarget.Value = nextMonthTarget.Value;
                        nextMonthTarget.Performance = target.Performance;
                        nextMonthTarget.YTD = target.YTD;
                        await AddTarget(nextMonthTarget);
                        //_repoTarget.Add(nextMonthTarget);
                    }
                    else
                    {
                        //checkExist_current.Value = nextMonthTarget.Value;
                        checkExist_current.Performance = target.Performance;
                        checkExist_current.YTD = target.YTD;
                        await UpdateTarget(checkExist_current);
                    }

                    if (checkExist_next == null)
                    {
                        nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month, 1);
                        nextMonthTarget.Id = 0;
                        nextMonthTarget.Value = nextMonthTarget.Value;
                        nextMonthTarget.Performance = 0;
                        nextMonthTarget.YTD = 0;
                        await AddTarget(nextMonthTarget);
                    }
                    else
                    {
                        checkExist_next.Value = nextMonthTarget.Value;
                        await UpdateTarget(checkExist_next);
                    }
                }
                
             
                _repoTargetYTD.Update(targetYTD);
                addActions.ForEach(item =>
                {
                    item.CreatedTime = model.CurrentTime;
                });

                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);
                var updatethisMonthAction = new List<Models.Action>();
                var addDoList = new List<Do>();
                var updatedoList = new List<Do>();
                foreach (var item in updateActionForThisMonth)
                {
                    var action = await _repoAction.FindAll(x => x.Id == item.ActionId).FirstOrDefaultAsync();
                    action.StatusId = item.StatusId;
                    updatethisMonthAction.Add(action);
                    var yearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
                    var monthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
                    var updateTime = new DateTime(yearResult, monthResult, 1);
                    if (item.DoId == 0)
                    {
                        var addDoItem = new Do(item.DoContent, item.ResultContent, item.Achievement, item.ActionId);
                        addDoItem.CreatedTime = updateTime;
                        addDoList.Add(addDoItem);
                    }
                    else
                    {
                        var doItem = await _repoDo.FindAll(x => x.Id == item.DoId).FirstOrDefaultAsync();
                        doItem.Content = item.DoContent;
                        doItem.Achievement = item.Achievement;
                        doItem.ReusltContent = item.ResultContent;
                        updatedoList.Add(doItem);
                    }
                    var ac_status = await _repoActionStatus.FindAll(x => x.ActionId == item.ActionId && x.CreatedTime.Month == monthResult && x.CreatedTime.Year == yearResult).FirstOrDefaultAsync();
                    if (ac_status == null)
                    {
                        var addItem = new ActionStatus
                        {
                            ActionId = item.ActionId,
                            StatusId = item.StatusId.ToInt(),
                            CreatedTime = updateTime
                        };
                        _repoActionStatus.Add(addItem);
                    } else
                    {
                        ac_status.Submitted = false;
                        _repoActionStatus.Update(ac_status);
                    }
                }
                //var update = await _repoActionStatus.FindAll(x => updateActionStatus.Contains(x.Id)).ToListAsync();
                //update.ForEach(item =>
                //{
                //    item.Submitted = model.Target.Submitted;
                //});
                //_repoActionStatus.UpdateRange(update);

                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updatethisMonthAction);

                _repoDo.AddRange(addDoList);
                _repoDo.UpdateRange(updatedoList);

                await _repoAction.SaveAll();


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
    }
}