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
        public async Task<List<Personal>> GetAllByAirportId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Personals.Where(x => x.airport_id == id).ToListAsync();
            }
        }
    }
}
