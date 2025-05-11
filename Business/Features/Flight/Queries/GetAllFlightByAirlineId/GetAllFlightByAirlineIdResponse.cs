using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public class GetAllFlightByAirlineIdResponse
    {
        public List<Entities.Flight>? entity { get; set; }
        public ResponseModel? response { get; set; }
        public bool error { get; set; } = true;
    }
}
