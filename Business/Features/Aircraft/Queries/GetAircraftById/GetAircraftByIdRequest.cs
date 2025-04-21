using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Aircraft.Queries.GetAircraftById
{
    public record GetAircraftByIdRequest(int id) : IRequest<GetAircraftByIdResponse>;
}
