using DataAccess.Abstract;
using MediatR;


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
