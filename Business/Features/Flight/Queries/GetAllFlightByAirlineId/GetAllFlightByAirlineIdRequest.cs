using MediatR;

namespace Business.Features.Flight.Queries.GetAllFlightByAirlineId
{
    public record GetAllFlightByAirlineIdRequest(int id) : IRequest<GetAllFlightByAirlineIdResponse>;
}
