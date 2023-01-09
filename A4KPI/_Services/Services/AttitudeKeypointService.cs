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

    public class AttitudeKeypointService : IAttitudeKeypointService
    {
        private readonly IAttitudeKeypointRepository _repoAttitudeKeypoint;
        private readonly IAttitudeCategoryRepository _repoAttitudeCategory;
        private readonly IAttitudeScoreRepository _repoAttitudeScore;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public AttitudeKeypointService(
            IAttitudeKeypointRepository repoAttitudeKeypoint,
            IAttitudeCategoryRepository repoAttitudeCategory,
            IAttitudeScoreRepository repoAttitudeScore,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repoAttitudeKeypoint = repoAttitudeKeypoint;
            _repoAttitudeCategory = repoAttitudeCategory;
            _repoAttitudeScore = repoAttitudeScore;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        public async Task<List<AttitudeKeypointDto>> GetAllByAttitudeScore(int attitudeHeadingID, int campaignID)
        {
            var attitudeCategories = _repoAttitudeCategory.FindAll(x => x.CampaignID == campaignID && x.AttitudeHeadingID == attitudeHeadingID).ToList();
            var attitudeKeypoints = new List<AttitudeKeypointDto>();

            foreach (var item in attitudeCategories)
            {
                var result = await _repoAttitudeKeypoint.FindAll(x => x.AttitudeCategoryID == item.ID)
                                                        .ProjectTo<AttitudeKeypointDto>(_configMapper)
                                                        .Select(x => new AttitudeKeypointDto{
                                                            ID = x.ID,
                                                            Name = x.Name,
                                                            AttitudeCategoryID = x.AttitudeHeadingID,
                                                            AttitudeCategoryName = item.Name,
                                                            Level = x.Level,
                                                            AttitudeHeadingID = x.AttitudeHeadingID
                                                        })
                                                        .ToListAsync();
                attitudeKeypoints.AddRange(result);
            }

            return attitudeKeypoints.OrderBy(x => x.AttitudeCategoryID).ToList();
            
        }

        public async Task<object> Add(AttitudeCategoryDto attitudeCategory)
        {
            var findAttitudeCategory = _repoAttitudeCategory.FindAll(x => x.CampaignID == attitudeCategory.CampaignID &&
                                                                        x.AttitudeHeadingID == attitudeCategory.AttitudeHeadingID &&
                                                                        x.Name == attitudeCategory.Name).FirstOrDefault();
            if (findAttitudeCategory == null)
            {
                var addAttitudeCategory = _mapper.Map<AttitudeCategory>(attitudeCategory);
                _repoAttitudeCategory.Add(addAttitudeCategory);
                await _repoAttitudeCategory.SaveAll();

                var newAttitudeCategory = await _repoAttitudeCategory.FindAll().OrderByDescending(x =>x.ID).FirstAsync();
                var addAttitudeKeypoint = _mapper.Map<AttitudeKeypoint>(attitudeCategory.AttitudeKeypoint);
                addAttitudeKeypoint.AttitudeCategoryID = newAttitudeCategory.ID;
                _repoAttitudeKeypoint.Add(addAttitudeKeypoint);
                await _repoAttitudeCategory.SaveAll();
            }
            else
            {
                var addAttitudeKeypoint = _mapper.Map<AttitudeKeypoint>(attitudeCategory.AttitudeKeypoint);
                addAttitudeKeypoint.AttitudeCategoryID = findAttitudeCategory.ID;
                _repoAttitudeKeypoint.Add(addAttitudeKeypoint);
                await _repoAttitudeCategory.SaveAll();
            }
            return new {    
                        status = true,
                        message = "Save Successfully"
                    };
            
        }


    }
}
