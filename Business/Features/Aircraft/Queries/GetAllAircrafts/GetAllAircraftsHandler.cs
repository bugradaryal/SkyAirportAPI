﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Aircraft.Queries.GetAircraftById;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Utilitys.ResponseHandler;

namespace Business.Features.Aircraft.Queries.GetAllAircrafts
{
    public class GetAllAircraftsHandler : IRequestHandler<GetAllAircraftsRequest, GetAllAircraftsResponse>
    {
        private readonly IAircraftRepository _aircraftRepository;
        public GetAllAircraftsHandler()
        {
            _aircraftRepository = new AircraftRepository();
        }

        public async Task<GetAllAircraftsResponse> Handle(GetAllAircraftsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var aircraft = await _aircraftRepository.GetAll();
                return new GetAllAircraftsResponse { entity = aircraft, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllAircraftsResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
