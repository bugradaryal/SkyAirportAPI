using MediatR;

namespace Business.Features.Airline.Queries
{
    public record GetAllAirlinesByAirportIdRequest(int id) : IRequest<GetAllAirlinesByAirportIdResponse>;
}
