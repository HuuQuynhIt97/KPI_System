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

    public class AttitudeHeadingService : IAttitudeHeadingService
    {
        private readonly IAttitudeHeadingRepository _repoAttitudeHeading;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public AttitudeHeadingService(
            IAttitudeHeadingRepository repoAttitudeHeading,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repoAttitudeHeading = repoAttitudeHeading;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        
        public async Task<List<AttitudeHeadingDto>> GetAllAsync()
        {
            return await _repoAttitudeHeading.FindAll().ProjectTo<AttitudeHeadingDto>(_configMapper).ToListAsync();
        }

    }
}
