using Business.Features.Crew.Qeeries;
using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Crew.Queries
{
    public class GetAllCrewByAircraftIdHandler : IRequestHandler<GetAllCrewByAircraftIdRequest, GetAllCrewByAircraftIdResponse>
    {
        private readonly ICrewRepository _crewRepository;
        public GetAllCrewByAircraftIdHandler(ICrewRepository crewRepository)
        {
            _crewRepository = crewRepository;
        }

        public async Task<GetAllCrewByAircraftIdResponse> Handle(GetAllCrewByAircraftIdRequest request, CancellationToken cancellationToken)
        {
            var crew = await _crewRepository.GetAllByAircraftId(request.id);
            return new GetAllCrewByAircraftIdResponse { entity = crew };
        }
    }
}
