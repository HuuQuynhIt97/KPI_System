using AutoMapper;
using A4KPI.Data;
using A4KPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.DTO;
using System.Net;
using A4KPI.Constants;
using A4KPI.Models;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
    
    public class ServiceBase<T, TDto> : IServiceBase<T, TDto> where T : class
    {
        private OperationResult operationResult;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<T> _repo;
        private readonly MapperConfiguration _configMapper;

        public ServiceBase(
            IRepositoryBase<T> repo,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _mapper = mapper;
            _repo = repo;
            _configMapper = configMapper;
        }

        public virtual async Task<OperationResult> AddAsync(TDto model)
        {
            var item = _mapper.Map<T>(model);
            _repo.Add(item);
            try
            {
                await _repo.SaveAll();
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
        public virtual async Task<OperationResult> AddRangeAsync(List<TDto> model)
        {
            var item = _mapper.Map<List<T>>(model);
            _repo.AddRange(item);
            try
            {
                await _repo.SaveAll();
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

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            var item = _repo.FindById(id);
            _repo.Remove(item);
            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
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

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<TDto>(_configMapper).ToListAsync();

        }

        public virtual TDto GetById(object id)
        {
            return _mapper.Map<T, TDto>(_repo.FindById(id));
        }
        public virtual async Task<TDto> GetByIdAsync(object id)
        {
            return  _mapper.Map<T, TDto>( await _repo.FindByIdAsync(id));
        }
        public virtual async Task<PagedList<TDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            var lists = _repo.FindAll().ProjectTo<TDto>(_configMapper).OrderByDescending(x => x.GetType().GetProperty("ID").GetValue(x));
            return await PagedList<TDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public virtual async Task<PagedList<TDto>> SearchAsync(PaginationParams param, object text)
        {
            var lists = _repo.FindAll().ProjectTo<TDto>(_configMapper)
          .Where(x => x.GetType().GetProperty("Name").GetValue(x) == text)
          .OrderByDescending(x => x.GetType().GetProperty("ID"));
            return await PagedList<TDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public virtual async Task<OperationResult> UpdateAsync(TDto model)
        {
            var item = _mapper.Map<T>(model);
            _repo.Update(item);
            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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
    
        public virtual async Task<OperationResult> UpdateRangeAsync(List<TDto> model)
        {
            var item = _mapper.Map<List<T>>(model);
            _repo.UpdateRange(item);
            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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

    }
}
