using DataAccess.Abstract;
using MediatR;


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
