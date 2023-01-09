using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using NetUtility;
using Microsoft.Extensions.Configuration;

namespace A4KPI._Repositories.Interface
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        
    }
    
}
