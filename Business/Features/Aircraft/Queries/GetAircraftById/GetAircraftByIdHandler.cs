using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Airline.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Account.Queries.Login;


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
