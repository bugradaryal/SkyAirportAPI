using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
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
        public GetAllCrewByAircraftIdHandler()
        {
            _crewRepository = new CrewRepository();
        }

        public async Task<GetAllCrewByAircraftIdResponse> Handle(GetAllCrewByAircraftIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var crew = await _crewRepository.GetAllByAircraftId(request.id);
                return new GetAllCrewByAircraftIdResponse { entity = crew, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllCrewByAircraftIdResponse { response = { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
