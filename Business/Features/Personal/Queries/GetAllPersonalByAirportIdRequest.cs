using MediatR;

namespace Business.Features.Personal.Queries
{
    public record GetAllPersonalByAirportIdRequest(int id) : IRequest<GetAllPersonalByAirportIdResponse>;
}
