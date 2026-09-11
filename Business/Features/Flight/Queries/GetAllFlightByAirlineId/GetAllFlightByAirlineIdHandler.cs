using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public class GetAllFlightByAirlineIdHandler : IRequestHandler<GetAllFlightByAirlineIdRequest, GetAllFlightByAirlineIdResponse>
    {
        private readonly IFlightRepository _flightRepository;
        public GetAllFlightByAirlineIdHandler(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<GetAllFlightByAirlineIdResponse> Handle(GetAllFlightByAirlineIdRequest request, CancellationToken cancellationToken)
        {
            var flights = await _flightRepository.GetAllByAirlineId(request.id);
            return new GetAllFlightByAirlineIdResponse { entity = flights };
        }
    }
}
