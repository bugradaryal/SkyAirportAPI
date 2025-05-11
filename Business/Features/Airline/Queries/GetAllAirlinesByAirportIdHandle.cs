using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Utilitys.ResponseHandler;

namespace Business.Features.Airline.Queries
{
    public class GetAllAirlinesByAirportIdHandle : IRequestHandler<GetAllAirlinesByAirportIdRequest, GetAllAirlinesByAirportIdResponse>
    {
        private readonly IAirlineRepository _airlineRepository;
        public GetAllAirlinesByAirportIdHandle()
        {
            _airlineRepository = new AirlineRepository();
        }

        public async Task<GetAllAirlinesByAirportIdResponse> Handle(GetAllAirlinesByAirportIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var airlines = await _airlineRepository.GetAllByAirportId(request.id);
                return new GetAllAirlinesByAirportIdResponse { entity = airlines, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllAirlinesByAirportIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
