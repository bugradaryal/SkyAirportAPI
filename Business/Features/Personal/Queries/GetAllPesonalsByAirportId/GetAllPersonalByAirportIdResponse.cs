using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;

namespace Business.Features.Personal.Queries.GetAllPesonalsByAirportId
{
    public class GetAllPersonalByAirportIdResponse
    {
        public List<Entities.Personal> entity {  get; set; }
        public CustomException exception {  get; set; }
        public bool error { get; set; } = true;

    }
}
