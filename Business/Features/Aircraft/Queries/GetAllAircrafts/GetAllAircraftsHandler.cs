using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Aircraft.Queries.GetAllAircrafts
{
    public class GetAllAircraftsHandler : IRequestHandler<GetAllAircraftsRequest, GetAllAircraftsResponse>
    {
        private readonly IAircraftRepository _aircraftRepository;
        public GetAllAircraftsHandler(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public async Task<GetAllAircraftsResponse> Handle(GetAllAircraftsRequest request, CancellationToken cancellationToken)
        {
            var aircraft = await _aircraftRepository.GetAll();
            return new GetAllAircraftsResponse { entity = aircraft };
        }
    }
}
