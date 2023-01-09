using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using A4KPI.Helpers;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
    
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IAccountTypeRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;

        public AccountTypeService(
            IAccountTypeRepository repo,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

      
    }
}
