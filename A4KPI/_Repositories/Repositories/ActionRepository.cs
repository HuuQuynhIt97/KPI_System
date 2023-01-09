
using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Repositories.Interface;
using A4KPI._Repositories.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace A4KPI._Repositories.Repositories
{

    public class ActionRepository : RepositoryBase<Models.Action>, IActionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ActionRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async  Task<object> SqlStoreProcedure(int kpiId)
        {
            var data = await _context.Actions.FromSqlRaw("Execute dbo.select_MeetingData_BykpiID_v3 @kpiid = {0} ,@subtract = {1} ,@plus = {2}", kpiId, 0, 0).ToListAsync();
            return data;
            //throw new System.NotImplementedException();
        }
    }

}
