using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;

namespace Business.Features.Aircraft.Queries.GetAircraftById
{
    public class GetAircraftByIdResponse
    {
        public List<Entities.Aircraft> entity { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
