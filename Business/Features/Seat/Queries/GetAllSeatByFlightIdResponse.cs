using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Seat.Queries
{
    public class GetAllSeatByFlightIdResponse
    {
        public List<Entities.Seat>? entity { get; set; }
        public ResponseModel? response { get; set; }
        public bool error { get; set; } = true;
    }
}
