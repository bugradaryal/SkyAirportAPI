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
    public class SeatRepository : ISeatRepository
    {
        public async Task<List<Seat>> GetAllByFlightId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Seats.Where(x => x.flight_id == id).ToListAsync();
            }
        }
    }
}
