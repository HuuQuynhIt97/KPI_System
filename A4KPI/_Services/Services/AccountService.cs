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
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IJobTitleRepository _repoJobTitle;
        private readonly IOCRepository _repoOc;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IKPINewRepository _repoKPINew;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IAccountGroupAccountRepository _repoAccountGroupAccount;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private readonly IAttitudeSubmitRepository _repoAttSubmit;
        private readonly ISystemFlowRepository _repoSystemFlow;
        private OperationResult operationResult;

        public AccountService(
            IAccountRepository repo,
            IAttitudeSubmitRepository repoAttSubmit,
            ISystemFlowRepository repoSystemFlow,
            IJobTitleRepository repoJobTitle,
            IOCRepository repoOC,
            IUserRoleRepository repoUserRole,
            IKPINewRepository repoKPINew,
            IKPIAccountRepository repoKPIAc,
            IRoleRepository repoRole,
            IAccountGroupAccountRepository repoAccountGroupAccount,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoSystemFlow = repoSystemFlow;
            _repoAttSubmit = repoAttSubmit;
            _repoJobTitle = repoJobTitle;
            _repoOc = repoOC;
            _repoKPINew = repoKPINew;
            _repoKPIAc = repoKPIAc;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public async Task<object> ChangePasswordAsync2(ChangePasswordRequest request)
        {
            var listEmail = new List<string>();
            var item = await _repo.FindByIdAsync(request.Id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này! Not found the account", Success = false };
            }
            item.Password = request.NewPassword.ToEncrypt();
            listEmail.Add(item.Email);
            try
            {
                _repo.Update(item);

                await _repo.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //operationResult = ex.GetMessageError();
            }
            //return operationResult;
        }

        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var listEmail = new List<string>();
            var item = await _repo.FindByIdAsync(request.Id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này! Not found the account", Success = false };
            }
            item.Password = request.NewPassword.ToEncrypt();
            try
            {
                _repo.Update(item);

                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Successfully 成功地!",
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

        public async Task<object> AddAsync(AccountDto model)
        {
            try
            {
                var accountCheck = _repo.FindAll(x => x.Username == model.Username).FirstOrDefault();
                if (accountCheck != null)
                {
                    operationResult = new OperationResult
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = MessageReponse.DataExist,
                        Success = false
                    };
                    return operationResult;
                }
                var item = _mapper.Map<Account>(model);
                item.Password = item.Password.ToEncrypt();
                _repo.Add(item);
                int id = await AddAccount(item);
                var list = new List<AccountGroupAccount>();
                list.Add(new AccountGroupAccount(1, id));
                //foreach (var accountGroupId in model.AccountGroupIds)
                //{
                //}

                _repoAccountGroupAccount.AddRange(list);
                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = id
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<int> AddAccount(Account item)
        {
            _repo.Add(item);
            await _repo.SaveAll();
            return item.Id;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(AccountDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                if (model.Password.Trim() != item.Password)
                    item.Password = model.Password.ToEncrypt();
                item.Username = model.Username;
                item.FullName = model.FullName;
                item.Email = model.Email;
                item.Leader = model.Leader;
                item.FactId = model.FactId;
                item.CenterId = model.CenterId;
                item.DeptId = model.DeptId;
                item.Manager = model.Manager;
                item.L1 = model.L1;
                item.L2 = model.L2;
                item.FunctionalLeader = model.FunctionalLeader;
                item.JobTitleId = model.JobTitleId;
                item.SystemFlow = model.SystemFlow;
                _repo.Update(item);

                var removingList = await _repoAccountGroupAccount.FindAll(x => x.AccountId == item.Id).ToListAsync();
                _repoAccountGroupAccount.RemoveMultiple(removingList);

                var list = new List<AccountGroupAccount>();
                list.Add(new AccountGroupAccount(1, item.Id));
                //foreach (var accountGroupId in model.AccountGroupIds)
                //{
                //}
                _repoAccountGroupAccount.AddRange(list);

                //update KPIAccountTable
                var list_kpiAc = _repoKPIAc.FindAll(x => x.AccountId == model.Id).ToList();
                var list_kpiAc_update = new List<KPIAccount>();

                if (list_kpiAc.Count > 0)
                {
                    foreach (var item_list in list_kpiAc)
                    {
                        item_list.DeptId = model.DeptId;
                        item_list.CenterId = model.CenterId;
                        item_list.FactId = model.FactId;
                        list_kpiAc_update.Add(item_list);
                    }
                }
                _repoKPIAc.UpdateRange(list_kpiAc_update);

                //update attitudeSubmit
                
                // var systemFlow_user = _repoSystemFlow.FindAll(x => x.SystemFlowID == model.SystemFlow).FirstOrDefault();
                // var list_update = new List<AttitudeSubmit>();
                // if (systemFlow_user != null)
                // {
                //     var att_submit = _repoAttSubmit.FindAll(x => x.SubmitTo == model.Id).ToList();
                //     foreach (var item_att in att_submit)
                //     {
                //         if (systemFlow_user.FL)
                //         {
                //             var item_add = new AttitudeSubmit
                //             {
                //                 ID = item_att.ID,
                //                 IsDisplayFL = systemFlow_user.FL,
                //                 IsDisplayL0 = systemFlow_user.L0,
                //                 IsDisplayL1 = systemFlow_user.L1,
                //                 IsDisplayL2 = systemFlow_user.L2,
                //                 SubmitTo = item_att.SubmitTo,
                //                 BtnFL = true,
                //                 BtnFLKPI = true,
                //                 CampaignID = item_att.CampaignID,
                //                 BtnNewAttFL = true,

                //             };
                //             list_update.Add(item_add);
                //         }
                //         else
                //         {
                //             var item_add = new AttitudeSubmit
                //             {
                //                 ID = item_att.ID,
                //                 SubmitTo = item_att.SubmitTo,
                //                 IsDisplayFL = systemFlow_user.FL,
                //                 BtnL0 = true,
                //                 BtnL0KPI = true,
                //                 IsDisplayL0 = systemFlow_user.L0,
                //                 IsDisplayL1 = systemFlow_user.L1,
                //                 IsDisplayL2 = systemFlow_user.L2,
                //                 CampaignID = item_att.CampaignID,
                //                 BtnNewAttL0 = true,

                //             };
                //             list_update.Add(item_add);
                //         }
                //     }
                // }
                // _repoAttSubmit.UpdateRange(list_update);
                await _repoAccountGroupAccount.SaveAll();

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

        public async Task<List<AccountDto>> GetAllAsync(string lang)
        {
            var query = _repo.FindAll(x => x.AccountType.Code != Systems.Administrator && x.AccountType.Code != Systems.SupremeAdmin);
            var model = from a in query
                        join b in query on a.Leader equals b.Id into ab
                        from ab1 in ab.DefaultIfEmpty()
                        join c in query on a.Manager equals c.Id into ac
                        from ac1 in ac.DefaultIfEmpty()
                        join d in _repoUserRole.FindAll() on a.Id equals d.UserID
                        join e in query on a.L1 equals e.Id into ae
                        from ae1 in ae.DefaultIfEmpty()
                        join f in query on a.L2 equals f.Id into af
                        from af1 in af.DefaultIfEmpty()
                        join g in query on a.FunctionalLeader equals g.Id into ag
                        from ag1 in ag.DefaultIfEmpty()
                        select new AccountDto
                        {
                            Id = a.Id,
                            Username = a.Username,
                            Password = a.Password,
                            FactId = a.FactId != null ? a.FactId : 0,
                            CenterId = a.CenterId != null ? a.CenterId : 0,
                            DeptId = a.DeptId != null ? a.DeptId : 0,
                            CreatedBy = a.CreatedBy,
                            CreatedTime = a.CreatedTime,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedTime = a.ModifiedTime,
                            IsLock = a.IsLock,
                            AccountTypeId = a.AccountTypeId,
                            AccountGroupIds = a.AccountGroupAccount.Count > 0 ? a.AccountGroupAccount.Select(x => x.AccountGroup.Id).ToList() : new List<int> { },
                            AccountGroupText = a.AccountGroupAccount.Count > 0 ? String.Join(",", a.AccountGroupAccount.Select(x => x.AccountGroup.Name)) : "",
                            FullName = a.FullName,
                            Email = a.Email,
                            LeaderName = ab1 != null ? ab1.FullName : "N/A",
                            Manager = a.Manager != null ? a.Manager : 0,
                            Leader = a.Leader != null ? a.Leader : 0,
                            ManagerName = ac1 != null ? ac1.FullName : "N/A",
                            FactName = a.FactId != null || a.FactId != 0 ? _repoOc.FindById(a.FactId).Name : "N/A",
                            CenterName = a.CenterId != null || a.CenterId != 0 ? _repoOc.FindById(a.CenterId).Name : "N/A",
                            DeptName = a.DeptId != null || a.DeptId != 0 ? _repoOc.FindById(a.DeptId).Name : "N/A",
                            Role = _repoRole.FindById(d.RoleID) != null ? _repoRole.FindById(d.RoleID).Name : "N/A",
                            RoleCode = _repoRole.FindById(d.RoleID) != null ? _repoRole.FindById(d.RoleID).Code : "N/A",
                            L0 = a.L0 != null ? a.L0 : false,
                            L1 = a.L1 != null ? a.L1 : 0,
                            L1Name = ae1 != null ? ae1.FullName : "N/A",
                            L2 = a.L2 != null ? a.L2 : 0,
                            L2Name = af1 != null ? af1.FullName : "N/A",
                            FunctionalLeader = a.FunctionalLeader != null ? a.FunctionalLeader : 0,
                            FunctionalLeaderName = ag1 != null ? ag1.FullName : "N/A",
                            GHR = a.GHR != null ? a.GHR : false,
                            JobTitleId = a.JobTitleId,
                            JobTitle = _repoJobTitle.FindById(a.JobTitleId) != null ? lang == SystemLang.EN ? _repoJobTitle.FindById(a.JobTitleId).NameEn : _repoJobTitle.FindById(a.JobTitleId).NameZh : "N/A",
                            GM = a.GM != null ? a.GM : false,
                            GMScore = !a.GMScore.IsNullOrEmpty() ? a.GMScore : false,
                            SystemFlow = a.SystemFlow != null ? a.SystemFlow : 0,
                        };
            var data = await model.ToListAsync();
            return data;

        }


        public async Task<OperationResult> LockAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.IsLock = !item.IsLock;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.IsLock ? MessageReponse.LockSuccess : MessageReponse.UnlockSuccess,
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

        public async Task<OperationResult> UpdateL0Async(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.L0 = !item.L0;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.L0.HasValue ? MessageReponse.L0Success : MessageReponse.UnL0Success,
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

        public async Task<OperationResult> UpdateGhrAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.GHR = !item.GHR;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.GHR.HasValue ? MessageReponse.GhrSuccess : MessageReponse.UnGhrSuccess,
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

        public async Task<OperationResult> UpdateGmAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.GM = !item.GM;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.GM.HasValue ? MessageReponse.UpdateSuccess : MessageReponse.UpdateError,
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

        public async Task<OperationResult> UpdateGmScoreAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.GMScore = item.GMScore.IsNullOrEmpty() ? true : !item.GMScore;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.GMScore.HasValue ? MessageReponse.UpdateSuccess : MessageReponse.UpdateError,
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

        public async Task<AccountDto> GetByUsername(string username)
        {
            var result = await _repo.FindAll(x => x.Username.ToLower() == username.ToLower()).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
            return result;
        }

        public async Task<object> GetAccounts()
        {
            var query = await _repo.FindAll(x => x.AccountType.Code != "SYSTEM").Select(x => new
            {
                x.Username,
                x.Id,
                x.FullName,
                IsLeader = x.AccountGroupAccount.Any(a => a.AccountGroup.Position == SystemRole.FunctionalLeader)
            }).ToListAsync();
            return query;
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var dataKPIAc = _repoKPINew.FindAll(x => x.CreateBy == id).ToList();
            if (dataKPIAc.Count > 0)
            {
                _repoKPINew.RemoveMultiple(dataKPIAc);
            }

            var delete = _repo.FindById(id);
            _repo.Remove(delete);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = delete
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
