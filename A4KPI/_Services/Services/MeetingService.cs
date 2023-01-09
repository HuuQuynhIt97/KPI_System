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

    public class MeetingService : IMeetingService
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
        public MeetingService(
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



        public async Task<MeetingAllDto> GetAllKPI(int userId)
        {
            var currentTime = DateTime.Now;
            var current_year = DateTime.Now.Year;
            var current_month = DateTime.Now.Month;
            var data = new List<MeetingDto>();
            var result = new List<MeetingDto>();
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
                    select new MeetingDto
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
                    PICNum = pics.Select(x => x.AccountId).ToList(),
                    TypeText = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Description,
                    PICName = _repoKPIAc.FindAll(y => y.KpiId == x.Id).ToList().Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                    FactName = String.Join(" , ", fact.Where(x => !String.IsNullOrEmpty(x))),
                    CenterName = String.Join(" , ", center.Where(x => !String.IsNullOrEmpty(x))),
                    DeptName = String.Join(" , ", dept.Where(x => !String.IsNullOrEmpty(x))),
                    }).OrderBy(x => x.Level).ToList();
            var model = data.Select(x => new MeetingDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Status = x.Status,
                PICName = x.PICName,
                TypeId = x.TypeId,
                Level = x.Level - 1,
                PICNum = x.PICNum,
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
             return new MeetingAllDto
            {
                result = result.Where(x => x.Year.Contains(current_year.ToString()) && x.Status == false).ToList(),
                FactName = fact_name,
                CenterName = center_name,
                DeptName = dept_name,
                FactId = user.FactId.IsNullOrEmpty() ? 0 : user.FactId,
                CenterId = user.CenterId.IsNullOrEmpty() ? 0 : user.CenterId,
                DeptId = user.DeptId.IsNullOrEmpty() ? 0 : user.DeptId
            };
        }

        public async Task<MeetingAllDto> GetAllKpiCHM(int userId)
        {
            var current_year = DateTime.Now.Year;
            var data = new List<MeetingDto>();
            var result = new List<MeetingDto>();
            data = (from x in (await _repoKPINew.FindAll().OrderBy(x => x.Name).ToListAsync())
                    join y in _repoKPIAc.FindAll() on x.Id equals y.KpiId into pics

                    let fact = pics.Select(x =>
                         _repoOC.FindById(x.FactId) != null ? _repoOC.FindById(x.FactId).Name : null)
                    let center = pics.Select(x =>
                       _repoOC.FindById(x.CenterId) != null ? _repoOC.FindById(x.CenterId).Name : null)
                    let dept = pics.Select(x =>
                      _repoOC.FindById(x.DeptId) != null ? _repoOC.FindById(x.DeptId).Name : null)
                    select new MeetingDto
                    {

                        Id = x.Id,
                        ParentId = x.ParentId,
                        Name = x.Name,
                        Year = x.Year != null ? x.Year : x.CreatedTime.Year.ToString(),
                        TypeId = x.TypeId,
                        TypeName = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Name,
                        Level = x.Level,
                        CreatedTime = x.CreatedTime,
                        Sequence = x.Sequence,
                        SequenceCHM = x.SequenceCHM,
                        PICNum = pics.Select(x => x.AccountId).ToList(),
                        TypeText = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Description,
                        PICName = _repoKPIAc.FindAll(y => y.KpiId == x.Id).ToList().Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                        FactName = String.Join(" , ", fact.Where(x => !String.IsNullOrEmpty(x))),
                        CenterName = String.Join(" , ", center.Where(x => !String.IsNullOrEmpty(x))),
                        DeptName = String.Join(" , ", dept.Where(x => !String.IsNullOrEmpty(x))),
                    }).OrderBy(x => x.Level).ToList();
            var model = data.Select(x => new MeetingDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                PICName = x.PICName,
                TypeId = x.TypeId,
                Level = x.Level,
                PICNum = x.PICNum,
                CreatedTime = x.CreatedTime,
                Sequence = x.Sequence,
                SequenceCHM = x.SequenceCHM,
                Year = x.Year,
                TypeName = x.TypeName,
                TypeText = x.TypeText,
                FactName = x.FactName,
                CenterName = x.CenterName,
                DeptName = x.DeptName,

            }).Where(x => x.Year == current_year.ToString() && x.Level != Level.Level_1 && !x.SequenceCHM.IsNullOrEmpty() && x.SequenceCHM > 0).OrderBy(x => x.SequenceCHM).ToList();

            return new MeetingAllDto
            {
                result = model,
            };
        }

        public async Task<List<MeetingDto>> GetAllKPIWithFilterQuery(MeetingFilterRequest request)
        {
            var result = await GetAllKPI(0);
            var result_empty = new List<MeetingDto>();
            
            if (request.Factory.IsNullOrEmpty() && request.Center.IsNullOrEmpty() && request.Dept.IsNullOrEmpty() && request.Level == 0)
            {
                return result.result.ToList();
            }


            if (!request.Factory.IsNullOrEmpty() && request.Center.IsNullOrEmpty() && request.Dept.IsNullOrEmpty() && request.Level == 0)
            {
                return result.result.FindAll(x => x.FactName.Contains(request.Factory)).ToList();
            }

            if (!request.Factory.IsNullOrEmpty() && !request.Center.IsNullOrEmpty() && request.Dept.IsNullOrEmpty() && request.Level == 0)
            {
                return result.result.FindAll(x => x.FactName.Contains(request.Factory) && x.CenterName.Contains(request.Center)).ToList();
            }

            if (!request.Factory.IsNullOrEmpty() && !request.Center.IsNullOrEmpty() && request.Dept.IsNullOrEmpty() && request.Level > 0)
            {
                return result.result.FindAll(x => x.FactName.Contains(request.Factory) && x.CenterName.Contains(request.Center) && x.Level == request.Level).ToList();
            }


            if (!request.Factory.IsNullOrEmpty() && !request.Center.IsNullOrEmpty() && !request.Dept.IsNullOrEmpty() && request.Level == 0)
            {
                return result.result.FindAll(x => x.FactName.Contains(request.Factory) && x.CenterName.Contains(request.Center) && x.DeptName.Contains(request.Dept)).ToList();
            }

            if (!request.Factory.IsNullOrEmpty() && !request.Center.IsNullOrEmpty() && !request.Dept.IsNullOrEmpty() && request.Level > 0)
            {
                return result.result.FindAll(x => x.FactName.Contains(request.Factory) && x.CenterName.Contains(request.Center) && x.DeptName.Contains(request.Dept) && x.Level == request.Level).ToList();
            }


            if (request.Level > 0)
            {
                return result.result.FindAll(x => x.Level == request.Level).ToList();
            }

            //throw new NotImplementedException();
            return result_empty;
        }

        public async Task<ChartDtoDateTime> GetChartWithDateTimeOld(int kpiId, DateTime currentTime)
        {
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var typeId = _repoKPINew.FindById(kpiId).TypeId;
            List<string> listLabels = new List<string>();
            List<int> listLabel = new List<int>();
            List<int> listLabelData = new List<int>();
            List<double> listTarget = new List<double>();
            List<double> listPerfomance = new List<double>();
            List<double> listYTD = new List<double>();
            var dataTable = new List<DataTable>();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiId).FirstOrDefaultAsync();
            var parentKpi = await _repoKPINew.FindAll(x => x.Id == kpiModel.ParentId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policy = parentKpi.Name;
            var data = await _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Year == thisYearResult).ToListAsync();

            for (int i = 1; i <= 12; i++)
            {
                listLabel.Add(i);
            }
            for (int i = 1; i <= thisMonthResult; i++)
            {
                listLabelData.Add(i);
            }

            foreach (var a in listLabel)
            {
                switch (a)
                {
                    case 1:
                        listLabels.Add("Jan");
                        break;
                    case 2:
                        listLabels.Add("Feb"); break;
                    case 3:
                        listLabels.Add("Mar"); break;
                    case 4:
                        listLabels.Add("Apr"); break;
                    case 5:
                        listLabels.Add("May");
                        break;
                    case 6:
                        listLabels.Add("Jun"); break;
                    case 7:
                        listLabels.Add("Jul"); break;
                    case 8:
                        listLabels.Add("Aug"); break;
                    case 9:
                        listLabels.Add("Sep");
                        break;
                    case 10:
                        listLabels.Add("Oct"); break;
                    case 11:
                        listLabels.Add("Nov"); break;
                    case 12:
                        listLabels.Add("Dec"); break;
                }
            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    double dataTarget = data.FirstOrDefault(x => x.TargetTime.Month == item).Value;
                    listTarget.Add(dataTarget);

                }
                else
                {
                    listTarget.Add(0);
                }

            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    var dataPerfomance = data.FirstOrDefault(x => x.TargetTime.Month == item).Performance;
                    listPerfomance.Add(dataPerfomance);

                }
                else
                {
                    listPerfomance.Add(0);
                }
            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    double dataYTD = data.FirstOrDefault(x => x.TargetTime.Month == item).YTD;
                    listYTD.Add(dataYTD);

                }
                else
                {
                    listYTD.Add(0);
                }

            }

            double YTD = 0;
            var YTDs = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).ToList();
            if (YTDs.Count > 0)
            {
                YTD = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).FirstOrDefault().Value;
            }
            double TargetYTD = 0;

            var TargetYTDs = await _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Month == thisMonthResult && x.CreatedTime.Year == thisYearResult).ToListAsync();
            if (TargetYTDs.Count > 0)
            {
                TargetYTD = _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Month == thisMonthResult && x.CreatedTime.Year == thisYearResult).FirstOrDefault().YTD;
            }

            foreach (var item in listLabelData)
            {
                var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
                var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };
                
                //var datass = _repoAction.

                var currentMonthData = new List<UpdatePDCADto>(); // list công việc tháng hiện tại
                var undoneList = new List<UpdatePDCADto>(); // list công việc chưa hoàn thành

                //start
                if (item == SystemMonth.Jan) // nếu là tháng 1 thì tìm list công việc chưa hoàn thành bắt đầu từ tháng 12 trở về trước của năm trước
                {
                    undoneList = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year < thisYearResult && x.CreatedTime.Month <= SystemMonth.Dec)
                        select new UpdatePDCADto
                        {
                            ActionId = a.Id,
                            StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < thisYearResult && x.CreatedTime.Month <= SystemMonth.Dec) ?
                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < thisYearResult && x.CreatedTime.Month <= SystemMonth.Dec).StatusId : null,
                            ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year < thisYearResult&& x.CreatedTime.Month <= SystemMonth.Dec) ?
                            a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year < thisYearResult && x.CreatedTime.Month <= SystemMonth.Dec).Id : null
                        }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
                }
                else
                {
                    //tìm list công việc của năm trước chưa hoàn thành => add vào undoneList
                    var undoneListPreiousYear = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year < thisYearResult && x.CreatedTime.Month <= SystemMonth.Dec)
                                                 select new UpdatePDCADto
                                                 {
                                                     ActionId = a.Id,
                                                     StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item).StatusId : null,
                                                     ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item) ?
                                                     a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item).Id : null
                                                 }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
                    if (undoneListPreiousYear.Count > 0)
                    {
                        undoneList.AddRange(undoneListPreiousYear);
                    }
                    //end

                    //tìm list công việc của năm hiện tại chưa hoàn thành => add vào undoneList
                    var undoneListCurrentYear = 
                    (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month < item)
                     select new UpdatePDCADto
                                  {
                                      ActionId = a.Id,
                                      StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item).StatusId : null,
                                      ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item).Id : null
                                  }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
                    undoneList.AddRange(undoneListCurrentYear);
                    //end
                }
                //end

                //start
                if (undoneList.Count > 0)
                {
                    //star => tìm list công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item)
                                        join c in _repoAcs.FindAll(x => x.CreatedTime.Month == item) on a.Id equals c.ActionId
                                        join b in _repoDo.FindAll(x => x.CreatedTime.Month == item) on a.Id equals b.ActionId into ab
                                        from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                            ActionId = a.Id,
                                            DoId = sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent = sub == null ? "" : sub.Content,
                                            ResultContent = sub == null ? "" : sub.ReusltContent,
                                            Achievement = sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = c.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= item).Status.Name.Trim(),
                                            Target = a.Target,
                                        }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();

                    //thêm list công việc chưa làm xong của tháng trước vào tháng hiện tại
                    foreach (var itemAcs in undoneList)
                    {
                        currentMonthData.Add(new UpdatePDCADto
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                            ActionId = itemAcs.ActionId,
                            Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                            CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                            DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().Content,
                            ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().ReusltContent,
                            Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().Achievement,
                            Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd"),
                            StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                            Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                        });
                    }
                }
                else
                {
                    //tìm công việc tháng hiện tại
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item)
                                        join c in _repoAcs.FindAll(x => x.CreatedTime.Month == item) on a.Id equals c.ActionId
                                        join b in _repoDo.FindAll(x => x.CreatedTime.Month == item) on a.Id equals b.ActionId into ab
                                        from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                            ActionId = a.Id,
                                            DoId = sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            DoContent = sub == null ? "" : sub.Content,
                                            ResultContent = sub == null ? "" : sub.ReusltContent,
                                            Achievement = sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = c.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == item).Status.Name.Trim(),
                                            Target = a.Target,

                                        }).Where(y => !hideStatus.Contains((int)y.StatusId)).ToList();
                }
                //end


                // list công việc tháng tiếp theo
                var nextMonthData = new List<UpdatePDCADto>(); 
                if (item == SystemMonth.Dec) // nếu là tháng 12 thì list công việc tháng tiếp theo sẽ là tháng 1
                {
                    nextMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult + 1 && x.CreatedTime.Month == SystemMonth.Jan)
                                     join b in _repoDo.FindAll(x => x.CreatedTime.Month == SystemMonth.Jan) on a.Id equals b.ActionId into ab
                                     from sub in ab.DefaultIfEmpty()
                                     select new UpdatePDCADto
                                     {
                                         Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                         ActionId = a.Id,
                                         DoId = sub == null ? 0 : sub.Id,
                                         Content = a.Content,
                                         DoContent = sub == null ? "" : sub.Content,
                                         ResultContent = sub == null ? "" : sub.ReusltContent,
                                         Achievement = sub == null ? "" : sub.Achievement,
                                         Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                         StatusId = a.StatusId,
                                         StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == item).Status.Name.Trim(),
                                         Target = a.Target

                                     }).ToList();
                }
                else 
                {

                    nextMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item + 1)
                                    join b in _repoDo.FindAll(x => x.CreatedTime.Month == item + 1) on a.Id equals b.ActionId into ab
                                    from sub in ab.DefaultIfEmpty()
                                    select new UpdatePDCADto
                                    {
                                        Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                        ActionId = a.Id,
                                        DoId = sub == null ? 0 : sub.Id,
                                        Content = a.Content,
                                        DoContent = sub == null ? "" : sub.Content,
                                        ResultContent = sub == null ? "" : sub.ReusltContent,
                                        Achievement = sub == null ? "" : sub.Achievement,
                                        Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                        StatusId = a.StatusId,
                                        StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == item).Status.Name.Trim(),
                                        Target = a.Target

                                    }).ToList();
                }
                // end công việc tháng tiếp theo


                var dataAdd = new DataTable()
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                    CurrentMonthData = currentMonthData.OrderBy(x => x.CreatedTime),
                    Date = $"{thisYearResult}/{item}/01",
                    KpiId = kpiId,
                    NextMonthData = nextMonthData
                };
                dataTable.Add(dataAdd);
            }


            return new ChartDtoDateTime
            {
                Status = true,
                labels = listLabels.ToArray(),
                perfomances = listPerfomance.ToArray(),
                targets = listTarget.ToArray(),
                ytds = listYTD.ToArray(),
                YTD = YTD,
                TypeId = typeId,
                TargetYTD = TargetYTD,
                DataTable = dataTable,
                Policy = policy
            };
        }

        public static IEnumerable<(string Month, int Year)> MonthsBetween( DateTime? startDate, DateTime? endDate)
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
        public async Task<ChartDtoDateTime> GetChartWithDateTime(int kpiId, DateTime currentTime)
        {
            var typeId = _repoKPINew.FindById(kpiId).TypeId;
            List<string> listLabels = new List<string>();
            List<int> listLabel = new List<int>();
            List<int> listLabelData = new List<int>();
            List<double> listTarget = new List<double>();
            List<double> listPerfomance = new List<double>();
            List<double> listYTD = new List<double>();
            var dataTable = new List<DataTable>();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiId).FirstOrDefaultAsync();
            var parentKpi = await _repoKPINew.FindAll(x => x.Id == kpiModel.ParentId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policy = parentKpi.Name;

            var data = await _repoTarget.FindAll(x => x.KPIId == kpiId).ToListAsync();

            List<int> month = new List<int>();

            //var dataStoreProcedure = _repoAction.SqlStoreProcedure(kpiId);
            if (kpiModel.StartDisplayMeetingTime == null && kpiModel.EndDisplayMeetingTime == null)
            {
                return new ChartDtoDateTime
                {
                    Status = false
                };
            }
            var months = MonthsBetween(kpiModel.StartDisplayMeetingTime, kpiModel.EndDisplayMeetingTime);

            foreach (var item in months)
            {
                int test = DateTime.ParseExact(item.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                listLabel.Add(test);
            }
            
            foreach (var a in listLabel)
            {
                switch (a)
                {
                    case 1:
                        listLabels.Add("Jan"); break;
                    case 2:
                        listLabels.Add("Feb"); break;
                    case 3:
                        listLabels.Add("Mar"); break;
                    case 4:
                        listLabels.Add("Apr"); break;
                    case 5:
                        listLabels.Add("May"); break;
                    case 6:
                        listLabels.Add("Jun"); break;
                    case 7:
                        listLabels.Add("Jul"); break;
                    case 8:
                        listLabels.Add("Aug"); break;
                    case 9:
                        listLabels.Add("Sep"); break;
                    case 10:
                        listLabels.Add("Oct"); break;
                    case 11:
                        listLabels.Add("Nov"); break;
                    case 12:
                        listLabels.Add("Dec"); break;
                }
            }

            foreach (var item in months)
            {
                var monthNum = DateTime.ParseExact(item.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                var yearNum = item.Year;
                var dataExist = data.Where(x => x.TargetTime.Month == monthNum && x.TargetTime.Year == yearNum).ToList();
                if (dataExist.Count > 0)
                {
                    double dataTarget = data.FirstOrDefault(x => x.TargetTime.Month == monthNum && x.TargetTime.Year == yearNum).Value;
                    listTarget.Add(dataTarget);

                    var dataPerfomance = data.FirstOrDefault(x => x.TargetTime.Month == monthNum && x.TargetTime.Year == yearNum).Performance;
                    listPerfomance.Add(dataPerfomance);

                    double dataYTD = data.FirstOrDefault(x => x.TargetTime.Month == monthNum && x.TargetTime.Year == yearNum).YTD;
                    listYTD.Add(dataYTD);
                }
                else
                {
                    listTarget.Add(0);
                    listPerfomance.Add(0);
                    listYTD.Add(0);
                }
            }

            double YTD = 0;
            var YTDs = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).ToList();

            if (YTDs.Count > 0)
            {
                YTD = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).FirstOrDefault().Value;
            }

            double TargetYTD = 0;

            foreach (var item in months.Where(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture).Month <= currentTime.Month + 1 && x.Year <= currentTime.Year))
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
                    undoneList = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year < yearNum - 1 && x.CreatedTime.Month <= SystemMonth.Dec)
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
                    var undoneListPreiousYear = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year < yearNum && x.CreatedTime.Month <= SystemMonth.Dec)
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
                    (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month < monthNum)
                     select new UpdatePDCADto
                     {
                         ActionId = a.Id,
                         CreatedBy = a.Account.FullName,
                         StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum) ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).StatusId : 0,
                         ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)  ?
                                      a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum).Id : 0,
                         IsDelete = a.ActionStatus.Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)  ?
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
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                        join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                        from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId = sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            CreatedTime = a.CreatedTime,
                                            DoContent = sub == null ? "" : sub.Content,
                                            ResultContent = sub == null ? "" : sub.ReusltContent,
                                            Achievement = sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                            IsDelete = a.ActionStatus.Any(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum) ?
                                            a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= monthNum).IsDelete : false,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end

                    //thêm list công việc chưa làm xong của tháng trước vào tháng hiện tại
                    foreach (var itemAcs in undoneList)
                    {
                        var check = _repoAcs.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault() != null ?
                        _repoAcs.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum - 1 && !x.IsDelete).FirstOrDefault().StatusId : 0;
                        if (!hideStatus.Contains((int)check) && check != 0)
                        {
                            try
                            {
                                currentMonthData.Add(new UpdatePDCADto
                                {
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                                    ActionId = itemAcs.ActionId,
                                    CreatedBy = itemAcs.CreatedBy,
                                    Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                                    CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                                    DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Content,
                                    ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().ReusltContent,
                                    Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == monthNum).FirstOrDefault().Achievement,
                                    Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.HasValue ? _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd") : null,
                                    StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                                    Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                                    IsDelete = _repoAcs.FindAll().Any(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId) ?
                                            _repoAcs.FindAll(x => x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == itemAcs.ActionId).FirstOrDefault().IsDelete : false
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
                    currentMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum)
                                        join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum) on a.Id equals b.ActionId into ab
                                        from sub in ab.DefaultIfEmpty()
                                        select new UpdatePDCADto
                                        {
                                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                                            ActionId = a.Id,
                                            CreatedBy = a.Account.FullName,
                                            DoId = sub == null ? 0 : sub.Id,
                                            Content = a.Content,
                                            DoContent = sub == null ? "" : sub.Content,
                                            ResultContent = sub == null ? "" : sub.ReusltContent,
                                            Achievement = sub == null ? "" : sub.Achievement,
                                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                            StatusId = a.StatusId,
                                            StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == monthNum).Status.Name.Trim(),
                                            Target = a.Target,
                                            IsDelete = a.ActionStatus.Any(x =>x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id) ?
                                            a.ActionStatus.FirstOrDefault(x =>x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum && x.ActionId == a.Id).IsDelete : false,
                                        }).Where(x => !x.IsDelete).ToList();
                    //end
                }
                //end

                var nextMonthData = new List<UpdatePDCADto>(); // list công việc tháng tiếp theo


                if (monthNum == SystemMonth.Dec) // nếu là tháng 12 thì list công việc tháng tiếp theo sẽ là tháng 1
                {
                    nextMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == yearNum + 1 && x.CreatedTime.Month == SystemMonth.Jan)
                                     join b in _repoDo.FindAll(x => x.CreatedTime.Month == SystemMonth.Jan) on a.Id equals b.ActionId into ab
                                     from sub in ab.DefaultIfEmpty()
                                     select new UpdatePDCADto
                                     {
                                         Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                                         ActionId = a.Id,
                                         CreatedBy = a.Account.FullName,
                                         DoId = sub == null ? 0 : sub.Id,
                                         Content = a.Content,
                                         DoContent = sub == null ? "" : sub.Content,
                                         ResultContent = sub == null ? "" : sub.ReusltContent,
                                         Achievement = sub == null ? "" : sub.Achievement,
                                         Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                         StatusId = a.StatusId,
                                         StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id).Status.Name.Trim(),
                                         IsDelete = a.ActionStatus.Any(x => x.ActionId == a.Id && x.CreatedTime.Year == yearNum + 1 && x.CreatedTime.Month == SystemMonth.Jan) ?
                                            a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Year == yearNum + 1 && x.CreatedTime.Month == SystemMonth.Jan).IsDelete : false,
                                         Target = a.Target

                                     }).Where(x => !x.IsDelete).ToList();
                }
                else
                {

                    nextMonthData = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum + 1)
                                     join b in _repoDo.FindAll(x => x.CreatedTime.Month == monthNum + 1) on a.Id equals b.ActionId into ab
                                     from sub in ab.DefaultIfEmpty()
                                     select new UpdatePDCADto
                                     {
                                         Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                                         ActionId = a.Id,
                                         CreatedBy = a.Account.FullName,
                                         DoId = sub == null ? 0 : sub.Id,
                                         Content = a.Content,
                                         DoContent = sub == null ? "" : sub.Content,
                                         ResultContent = sub == null ? "" : sub.ReusltContent,
                                         Achievement = sub == null ? "" : sub.Achievement,
                                         Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                         StatusId = a.StatusId,
                                         StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == monthNum).Status.Name.Trim(),
                                         IsDelete = a.ActionStatus.Any(x => x.ActionId == a.Id && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum + 1) ?
                                            a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Year == yearNum && x.CreatedTime.Month == monthNum + 1).IsDelete : false,
                                         Target = a.Target

                                     }).Where(x => !x.IsDelete).ToList();
                }



                var dataAdd = new DataTable()
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum),
                    CurrentMonthData = currentMonthData.Where(x => !x.IsDelete).OrderBy(x => x.CreatedTime),
                    Date = monthNum < 10 ? $"{yearNum}/0{monthNum}/01" : $"{yearNum}/{monthNum}/01",
                    KpiId = kpiId,
                    NextMonthData = nextMonthData.Where(x => !x.IsDelete)
                };
                dataTable.Add(dataAdd);
            }


            return new ChartDtoDateTime
            {
                Status = true,
                labels = listLabels.ToArray(),
                perfomances = listPerfomance.ToArray(),
                targets = listTarget.ToArray(),
                ytds = listYTD.ToArray(),
                YTD = YTD,
                TypeId = typeId,
                TargetYTD = TargetYTD,
                DataTable = dataTable,
                Policy = policy
            };
        }

        public async Task<object> GetAllLevel()
        {
            var data = await _repoOC.FindAll(x => x.Level < Level.Level_4).Select(x => new { 
                id = x.Level,
                name = $"Level {x.Level}"
            }).ToListAsync();
            return data.DistinctBy(x => x.name);
        }
    }
}
