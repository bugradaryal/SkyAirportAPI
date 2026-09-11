using MediatR;

namespace Business.Features.Aircraft.Queries.GetAllAircrafts
{
    public record GetAllAircraftsRequest : IRequest<GetAllAircraftsResponse>;
}
