using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Personal.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Utilitys.ResponseHandler;

namespace Business.Features.OperationalDelay.Queries
{
    public class GetAllOperationalDelayByFlightIdHandler : IRequestHandler<GetAllOperationalDelayByFlightIdRequest, GetAllOperationalDelayByFlightIdResponse>
    {
        private readonly IOperationalDelayRepository _operationalDelayRepository;
        public GetAllOperationalDelayByFlightIdHandler()
        {
            _operationalDelayRepository = new OperationalDelayRepository();
        }

        public async Task<GetAllOperationalDelayByFlightIdResponse> Handle(GetAllOperationalDelayByFlightIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var op_delay = await _operationalDelayRepository.GetAllByFlightId(request.id);
                return new GetAllOperationalDelayByFlightIdResponse { entity = op_delay, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllOperationalDelayByFlightIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
