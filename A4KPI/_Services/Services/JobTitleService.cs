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

    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _repoJobTitle;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public JobTitleService(
            IJobTitleRepository repoJobTitle,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repoJobTitle = repoJobTitle;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        public async Task<List<JobTitleDto>> GetAllAsync()
        {
            return await _repoJobTitle.FindAll().ProjectTo<JobTitleDto>(_configMapper).ToListAsync();
        }

        public async Task<List<JobTitleDto>> GetAllByLangAsync(string lang)
        {
            var data = await _repoJobTitle.FindAll().Select(x => new JobTitleDto 
            {
                Id = x.Id,
                JobTitle = lang == SystemLang.EN ? x.NameEn : x.NameZh
            }).ToListAsync();
            return data;
        }

        public async Task<OperationResult> AddAsync(JobTitleDto model)
        {
            var add = _mapper.Map<JobTitle>(model);
            _repoJobTitle.Add(add);

            try
            {
                await _repoJobTitle.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = add
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateAsync(JobTitleDto model)
        {

            var update = _mapper.Map<JobTitle>(model);
            _repoJobTitle.Update(update);

            try
            {
                await _repoJobTitle.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = update
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
            
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var item = _repoJobTitle.FindById(id);
            _repoJobTitle.Remove(item);

            try
            {
                await _repoJobTitle.SaveAll();
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
