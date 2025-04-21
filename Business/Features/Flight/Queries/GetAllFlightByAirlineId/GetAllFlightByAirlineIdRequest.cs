using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public record GetAllFlightByAirlineIdRequest(int id) : IRequest<GetAllFlightByAirlineIdResponse>;
}
