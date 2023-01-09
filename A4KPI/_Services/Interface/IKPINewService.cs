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

namespace A4KPI._Services.Interface
{
    public interface IKPINewService
    {
        Task<object> GetListPic();
        Task<object> GetAllType(string lang);
        Task<bool> Delete(int id);
        Task<bool> IsDisPlayTodoUpdate(int ID);
        Task<OperationResult> AddAsync(KPINewDto model);
        Task<OperationResult> UpdateAsync(KPINewDto model);
        Task<OperationResult> UpdateSequence(KPINewDto model);
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView(string lang);
        Task<object> GetAllAsTreeView2nd3rd(string lang, int userId);
        Task<byte[]> ExportExcelKpiNew(string lang);
    }
    
}
