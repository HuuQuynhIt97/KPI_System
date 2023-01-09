using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;
using A4KPI.Helpers;
using A4KPI._Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace A4KPI._Services.Services
{
   
    public class VersionService :  IVersionService
    {
        private readonly IVersionRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public VersionService(
            IVersionRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<bool> Add(Models.Version model)
        {
            model.CreatedTime = DateTime.Now;
            _repo.Add(model);
            return await _repo.SaveAll();
        }

        public async Task<bool> Delete(int id)
        {
            var Version = _repo.FindById(id);
            _repo.Remove(Version);
            return await _repo.SaveAll();
        }

        public async Task<List<Models.Version>> GetAllAsync()
        {
            return await _repo.FindAll().OrderByDescending(x => x.ID).ToListAsync();
        }

        public Models.Version GetById(int id) => _repo.FindById(id);

        public async Task<PagedList<Models.Version>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repo.FindAll().OrderByDescending(x => x.ID);
            return await PagedList<Models.Version>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<PagedList<Models.Version>> Search(PaginationParams param, object text)
        {
            var lists = _repo.FindAll()
            .Where(x => x.Name.Contains(text.ToString()))
            .OrderByDescending(x => x.ID);
            return await PagedList<Models.Version>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<bool> Update(Models.Version model)
        {
            model.UpdatedTime = DateTime.Now;
            _repo.Update(model);
            return await _repo.SaveAll();
        }
    }
}
