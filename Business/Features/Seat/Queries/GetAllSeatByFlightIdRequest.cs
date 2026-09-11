using MediatR;

namespace Business.Features.Seat.Queries
{
    public record GetAllSeatTicketRequest(int id) : IRequest<GetAllSeatTicketResponse>;
}
