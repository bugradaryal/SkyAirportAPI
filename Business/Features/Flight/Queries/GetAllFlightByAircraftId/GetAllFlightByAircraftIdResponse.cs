using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;

namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public class GetAllFlightByAircraftIdResponse
    {
        public List<Entities.Flight> entity { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
