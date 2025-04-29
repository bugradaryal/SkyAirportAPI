using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;

namespace Business.Features.Seat.Queries
{
    public class GetAllSeatByFlightIdResponse
    {
        public List<Entities.Seat> entity { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
