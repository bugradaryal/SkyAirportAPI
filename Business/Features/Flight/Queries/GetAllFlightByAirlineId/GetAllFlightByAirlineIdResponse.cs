using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public class GetAllFlightByAirlineIdResponse
    {
        public List<Entities.Flight>? entity { get; set; }
    }
}
