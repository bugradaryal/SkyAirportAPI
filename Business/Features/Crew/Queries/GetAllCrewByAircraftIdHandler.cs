using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Airline.Queries;
using Business.Features.Crew.Qeeries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;


namespace Business.Features.Crew.Queries
{
    public class GetAllCrewByAircraftIdHandler : IRequestHandler<GetAllCrewByAircraftIdRequest,GetAllCrewByAircraftIdResponse>
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
