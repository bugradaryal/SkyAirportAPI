using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public class GetAllFlightByAircraftIdResponse
    {
        public List<Entities.Flight>? entity { get; set; }
    }
}
