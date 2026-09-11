using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class PersonalRepository : IPersonalRepository
    {
        private DataDbContext _dbContext;
        public PersonalRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Personal>> GetAllByAirportId(int id)
        {
            return await _dbContext.Personals.Where(x => x.airport_id == id).ToListAsync();
        }
    }
}
