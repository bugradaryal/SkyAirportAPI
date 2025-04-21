using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Crew.Queries;
using MediatR;

namespace Business.Features.Crew.Qeeries
{
    public record GetAllCrewByAircraftIdRequest(int id) : IRequest<GetAllCrewByAircraftIdResponse>;
}
