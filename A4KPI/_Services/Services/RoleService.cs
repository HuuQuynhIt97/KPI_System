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
    
    public class RoleService :  IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IOCRepository _repoOc;
        private readonly IAccountGroupAccountRepository _repoAccountGroupAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public RoleService(
            IRoleRepository repo,
            IOCRepository repoOC,
            IUserRoleRepository repoUserRole,
            IAccountGroupAccountRepository repoAccountGroupAccount,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOc = repoOC;
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

        public async Task<object> GetRoleByUserID(int userid)
        {
            try
            {
                var model = await _repoUserRole.FindAll().Include(x => x.Role).FirstOrDefaultAsync(x => x.UserID == userid);
                return model.Role;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public  async Task<OperationResult> AddAsync(RoleDto model)
        {
            try
            {
                var artRole = _mapper.Map<Role>(model);
                _repo.Add(artRole);
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
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  async Task<OperationResult> UpdateAsync(RoleDto model)
        {
            try
            {
                var artRole = _mapper.Map<Role>(model);
                _repo.Update(artRole);
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

        public  async Task<List<RoleDto>> GetAllAsync()
        {
            return await _repo.FindAll(x => x.Code != "SUPPER_ADMIN").ProjectTo<RoleDto>(_configMapper).OrderBy(x => x.ID).ToListAsync();
        }

        public async Task<object> MapUserRole(int userID, int roleID)
        {
            var item = await _repoUserRole.FindAll(x => x.UserID == userID).ToListAsync();
            if (item.Count == 0)
            {
                try
                {
                    _repoUserRole.Add(new UserRole
                    {
                        UserID = userID,
                        RoleID = roleID,
                    });

                    return new
                    {
                        status = await _repoUserRole.SaveAll(),
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
            else
            {

                try
                {
                    await RemoveRole(item);
                    _repoUserRole.Add(new UserRole
                    {
                        UserID = userID,
                        RoleID = roleID,
                    });

                    return new
                    {
                        status = await _repoUserRole.SaveAll(),
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveRole(List<UserRole> item)
        {
            _repoUserRole.RemoveMultiple(item);
            await _repoUserRole.SaveAll();
            return true;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Delegate.Combine();
            try
            {
                var model = _repo.FindById(id);
                _repo.Remove(model);
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
    }
}
