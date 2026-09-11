using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Aircraft.Queries.GetAircraftById
{
    public class GetAircraftByIdHandler : IRequestHandler<GetAircraftByIdRequest, GetAircraftByIdResponse>
    {
        private readonly IAircraftRepository _aircraftRepository;
        public GetAircraftByIdHandler(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public async Task<GetAircraftByIdResponse> Handle(GetAircraftByIdRequest request, CancellationToken cancellationToken)
        {
            var aircraft = await _aircraftRepository.GetAllById(request.id);
            return new GetAircraftByIdResponse { entity = aircraft };
        }
    }
}
