using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Aircraft.Queries.GetAllAircrafts
{
    public record GetAllAircraftsRequest : IRequest<GetAllAircraftsResponse>;
}
