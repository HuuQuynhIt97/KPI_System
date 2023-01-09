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

namespace A4KPI._Services.Interface
{
    public interface IFunctionSystemService
    {
        Task<List<Models.Version>> GetAllAsync();
        Task<PagedList<Models.Version>> Search(PaginationParams param, object text);
        Models.Version GetById(int id);
        Task<bool> Add(Models.Version model);
        Task<bool> Update(Models.Version model);
        Task<bool> Delete(int id);
        Task<PagedList<Models.Version>> GetWithPaginations(PaginationParams param);
    }
    
}
