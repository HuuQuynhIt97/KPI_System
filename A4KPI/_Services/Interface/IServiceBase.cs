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

namespace A4KPI._Services.Interface
{
    public interface IServiceBase<T, TDto>
    {
        Task<OperationResult> AddAsync(TDto model);
        Task<OperationResult> AddRangeAsync(List<TDto> model);


        Task<OperationResult> UpdateAsync(TDto model);
        Task<OperationResult> UpdateRangeAsync(List<TDto> model);

        Task<OperationResult> DeleteAsync(int id);

        Task<List<TDto>> GetAllAsync();

        Task<PagedList<TDto>> GetWithPaginationsAsync(PaginationParams param);

        Task<PagedList<TDto>> SearchAsync(PaginationParams param, object text);
        TDto GetById(object id);
        Task<TDto> GetByIdAsync(object id);
    }
   
}
