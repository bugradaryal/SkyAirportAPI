using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Airline.Queries
{
    public record GetAllAirlinesByAirportIdRequest(int id) : IRequest<GetAllAirlinesByAirportIdResponse>;
}
