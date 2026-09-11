using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Aircraft.Queries.GetAircraftById;
using DataAccess.Abstract;
using DataAccess.Concrete;
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
