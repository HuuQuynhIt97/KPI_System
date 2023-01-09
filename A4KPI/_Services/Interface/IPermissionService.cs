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
using NetUtility;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Interface
{
    public interface IPermissionService
    {
        Task<object> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request);
        Task<ResponseDetail<object>> PutPermissionByRoleId(int roleID, UpdatePermissionRequest request);
        Task<object> GetMenuByLangID(int userId, string langID);
        Task<object> GetActionInFunctionByRoleID(int roleID);
    }
    
}
