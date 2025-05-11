using System;
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
using Business.Features.Account.Queries.Login;
using Utilitys.ResponseHandler;

namespace Business.Features.Aircraft.Queries.GetAircraftById
{
    public class GetAircraftByIdHandler : IRequestHandler<GetAircraftByIdRequest, GetAircraftByIdResponse>
    {
        private readonly IAircraftRepository _aircraftRepository;
        public GetAircraftByIdHandler()
        {
            _aircraftRepository = new AircraftRepository();
        }

        public async Task<GetAircraftByIdResponse> Handle(GetAircraftByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var aircraft = await _aircraftRepository.GetAllById(request.id);
                return new GetAircraftByIdResponse { entity = aircraft, error = false };
            }
            catch (Exception ex)
            {
                return new GetAircraftByIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
