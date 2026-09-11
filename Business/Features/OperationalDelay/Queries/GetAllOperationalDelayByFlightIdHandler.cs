using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Personal.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;


namespace Business.Features.OperationalDelay.Queries
{
    public class GetAllOperationalDelayByFlightIdHandler : IRequestHandler<GetAllOperationalDelayByFlightIdRequest, GetAllOperationalDelayByFlightIdResponse>
    {
        private readonly IOperationalDelayRepository _operationalDelayRepository;
        public GetAllOperationalDelayByFlightIdHandler(IOperationalDelayRepository operationalDelayRepository)
        {
            _operationalDelayRepository = operationalDelayRepository;
        }

        public async Task<GetAllOperationalDelayByFlightIdResponse> Handle(GetAllOperationalDelayByFlightIdRequest request, CancellationToken cancellationToken)
        {
            var op_delay = await _operationalDelayRepository.GetAllByFlightId(request.id);
            return new GetAllOperationalDelayByFlightIdResponse { entity = op_delay };
        }
    }
}
