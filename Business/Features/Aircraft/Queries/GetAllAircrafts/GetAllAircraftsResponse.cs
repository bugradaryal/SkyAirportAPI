using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Aircraft.Queries.GetAllAircrafts
{
    public class GetAllAircraftsResponse
    {
        public List<Entities.Aircraft>? entity { get; set; }
    }
}
