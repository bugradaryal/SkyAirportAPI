using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;


namespace Business.Features.Airline.Queries
{
    public class GetAllAirlinesByAirportIdHandle : IRequestHandler<GetAllAirlinesByAirportIdRequest, GetAllAirlinesByAirportIdResponse>
    {
        private readonly IAirlineRepository _airlineRepository;
        public GetAllAirlinesByAirportIdHandle(IAirlineRepository airlineRepository)
        {
            _airlineRepository = airlineRepository;
        }

        public async Task<GetAllAirlinesByAirportIdResponse> Handle(GetAllAirlinesByAirportIdRequest request, CancellationToken cancellationToken)
        {
            var airlines = await _airlineRepository.GetAllByAirportId(request.id);
            return new GetAllAirlinesByAirportIdResponse { entity = airlines };
        }
    }
}
