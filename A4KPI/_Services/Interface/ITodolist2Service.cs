using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
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
using System.Globalization;

namespace A4KPI._Services.Interface
{
    public interface IToDoList2Service
    {
        Task<OperationResult> SubmitAction(ActionRequestDto model);
        Task<OperationResult> SaveAction(ActionRequestDto model);
        Task<OperationResult> SubmitUpdatePDCA(PDCARequestDto model);
        Task<OperationResult> SaveUpdatePDCA(PDCARequestDto model);
        Task<object> GetStatus();
        Task<object> L0(DateTime currentTime, int userId);
        Task<object> L0Revise(DateTime currentTime, int userId);
        Task<object> GetActionsForL0(int kpiNewId, int userId);
        Task<bool> Delete(int id);

        Task<object> GetPDCAForL0(int kpiNewId, DateTime currentTime, int userId);
        Task<object> GetPDCAForL0Revise(int kpiNewId, DateTime currentTime, int userId);
        Task<object> GetTargetForUpdatePDCA(int kpiNewId, DateTime currentTime);
        Task<object> GetActionsForUpdatePDCA(int kpiNewId, DateTime currentTime , int userId);
        Task<object> GetKPIForUpdatePDC(int kpiNewId, DateTime currentTime);
        Task<OperationResult> SubmitKPINew(int kpiNewId);

        Task<OperationResult> AddOrUpdateStatus(ActionStatusRequestDto request);


    }
    
}