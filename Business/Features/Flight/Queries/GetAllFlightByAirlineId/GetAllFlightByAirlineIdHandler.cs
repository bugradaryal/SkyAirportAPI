﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Airline.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Utilitys.ResponseHandler;

namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public class GetAllFlightByAirlineIdHandler : IRequestHandler<GetAllFlightByAirlineIdRequest, GetAllFlightByAirlineIdResponse>
    {
        private readonly IFlightRepository _flightRepository;
        public GetAllFlightByAirlineIdHandler()
        {
            _flightRepository = new FlightRepository();
        }

        public async Task<GetAllFlightByAirlineIdResponse> Handle(GetAllFlightByAirlineIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var flights = await _flightRepository.GetAllByAirlineId(request.id);
                return new GetAllFlightByAirlineIdResponse { entity = flights, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllFlightByAirlineIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
