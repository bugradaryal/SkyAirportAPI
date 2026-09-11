using Business.Features.Crew.Queries;
using MediatR;

namespace Business.Features.Crew.Qeeries
{
    public record GetAllCrewByAircraftIdRequest(int id) : IRequest<GetAllCrewByAircraftIdResponse>;
}
