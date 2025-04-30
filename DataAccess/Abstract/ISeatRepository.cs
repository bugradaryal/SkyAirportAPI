using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess.Abstract
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllByFlightId(int id);
        Task<Aircraft> GetSeatAndAircraftByTicketId(int id);
        Task<bool> IsSeatAvailable(int id);
        Task SetSeatAvailable(int id, bool value);
    }
}
