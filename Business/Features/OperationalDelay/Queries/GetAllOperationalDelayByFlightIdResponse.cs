using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.OperationalDelay.Queries
{
    public class GetAllOperationalDelayByFlightIdResponse
    {
        public List<Entities.OperationalDelay>? entity { get; set; }
    }
}
