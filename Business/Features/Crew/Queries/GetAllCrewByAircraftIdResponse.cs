using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Crew.Queries
{
    public class GetAllCrewByAircraftIdResponse
    {
        public List<Entities.Crew> entity { get; set; }
        public ResponseModel response { get; set; }
        public bool error { get; set; } = true;

    }
}
