using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Seat.Queries
{
    public class GetAllSeatTicketResponse
    {
        public List<Entities.Seat>? entity { get; set; }
    }
}
