using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.OperationalDelay.Queries
{
    public record GetAllOperationalDelayByFlightIdRequest(int id) : IRequest<GetAllOperationalDelayByFlightIdResponse>;
}
