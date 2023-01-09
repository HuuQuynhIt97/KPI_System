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
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
  
    public class OCService : IOCService
    {
        private OperationResult operationResult;

        private readonly IOCRepository _repo;
        private readonly IAccountRepository _repoAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCService(
            IOCRepository repo,
            IAccountRepository repoAccount,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _mapper = mapper;
            _configMapper = configMapper;
        }


        public async Task<List<OCDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<OCDto>(_configMapper)
                .OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<object> GetAllLevel3()
        {
            var lists = (await _repo.FindAll(x => x.Level == Level.Level_3).ToListAsync());
            return lists;
        }

        public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<OCDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }


        public async Task<OperationResult> AddAsync(OCDto model)
        {
            var add = _mapper.Map<OC>(model);
            _repo.Add(add);

            try
            {
                await _repo.SaveAll();
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

      

        public async Task<OperationResult> UpdateAsync(OCDto model)
        {

            var update = _mapper.Map<OC>(model);
            _repo.Update(update);

            try
            {
                await _repo.SaveAll();
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
            var item = _repo.FindById(id);
            _repo.Remove(item);

            var itemChild = _repo.FindAll(x => x.ParentId == id).ToList();
            var itemSubChild = new List<OC>();

            if (itemChild != null)
            {
                foreach (var items in itemChild)
                {
                    var itemSubChilds = _repo.FindAll(x => x.ParentId == items.Id).ToList();
                    if (itemSubChilds != null)
                    {
                        itemSubChild.AddRange(itemSubChilds);
                    }
                }
                _repo.RemoveMultiple(itemSubChild);
                _repo.RemoveMultiple(itemChild);
            }
            try
            {
                await _repo.SaveAll();
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
