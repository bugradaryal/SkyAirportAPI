using MediatR;

namespace Business.Features.Flight.Queries.GetAllFlightByAircraftId
{
    public record GetAllFlightByAircraftIdRequest(int id) : IRequest<GetAllFlightByAircraftIdResponse>;
}
