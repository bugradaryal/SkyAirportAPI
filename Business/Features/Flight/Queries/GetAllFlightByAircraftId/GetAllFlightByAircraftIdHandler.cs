using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Utilitys.ResponseHandler;

namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public class GetAllFlightByAircraftIdHandler : IRequestHandler<GetAllFlightByAircraftIdRequest, GetAllFlightByAircraftIdResponse>
    {
        private readonly IFlightRepository _flightRepository;
        public GetAllFlightByAircraftIdHandler()
        {
            _flightRepository = new FlightRepository();
        }

        public async Task<GetAllFlightByAircraftIdResponse> Handle(GetAllFlightByAircraftIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var flights = await _flightRepository.GetAllByAircraftId(request.id);
                return new GetAllFlightByAircraftIdResponse { entity = flights, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllFlightByAircraftIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
