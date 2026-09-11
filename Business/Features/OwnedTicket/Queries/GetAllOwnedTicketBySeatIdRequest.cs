using MediatR;

namespace Business.Features.OwnedTicket.Queries
{
    public record GetAllOwnedTicketBySeatIdRequest(int id) : IRequest<GetAllOwnedTicketBySeatIdResponse>;
}
