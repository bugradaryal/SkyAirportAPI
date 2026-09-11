using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Crew.Queries
{
    public class GetAllCrewByAircraftIdResponse
    {
        public List<Entities.Crew>? entity { get; set; }

    }
}
