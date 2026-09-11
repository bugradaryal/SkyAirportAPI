using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;


namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public class GetAllFlightByAircraftIdHandler : IRequestHandler<GetAllFlightByAircraftIdRequest, GetAllFlightByAircraftIdResponse>
    {
        private readonly IFlightRepository _flightRepository;
        public GetAllFlightByAircraftIdHandler(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<GetAllFlightByAircraftIdResponse> Handle(GetAllFlightByAircraftIdRequest request, CancellationToken cancellationToken)
        {
            var flights = await _flightRepository.GetAllByAircraftId(request.id);
            return new GetAllFlightByAircraftIdResponse { entity = flights };
        }
    }
}
