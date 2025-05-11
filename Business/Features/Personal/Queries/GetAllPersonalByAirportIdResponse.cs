using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Personal.Queries
{
    public class GetAllPersonalByAirportIdResponse
    {
        public List<Entities.Personal>? entity {  get; set; }
        public ResponseModel? response {  get; set; }
        public bool error { get; set; } = true;

    }
}
