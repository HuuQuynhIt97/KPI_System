using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Repositories;

namespace A4KPI._Repositories.Repositories
{
   

    public class AccountGroupRepository : RepositoryBase<AccountGroup>, IAccountGroupRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AccountGroupRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
