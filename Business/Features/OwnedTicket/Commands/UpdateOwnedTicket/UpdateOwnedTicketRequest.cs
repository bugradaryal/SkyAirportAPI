using MediatR;


namespace Business.Features.OwnedTicket.Commands.UpdateOwnedTicket
{
    public record UpdateOwnedTicketRequest(Entities.OwnedTicket Ticket) : IRequest;
}
