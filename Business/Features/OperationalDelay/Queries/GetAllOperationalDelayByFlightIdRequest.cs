using MediatR;

namespace Business.Features.OperationalDelay.Queries
{
    public record GetAllOperationalDelayByFlightIdRequest(int id) : IRequest<GetAllOperationalDelayByFlightIdResponse>;
}
