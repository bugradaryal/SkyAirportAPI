using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using MediatR;

namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public record GetAllFlightByAircraftIdRequest(int id) : IRequest<GetAllFlightByAircraftIdResponse>;
}
