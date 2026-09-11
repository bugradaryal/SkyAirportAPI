using MediatR;


namespace Business.Features.OwnedTicket.Commands.AddOwnedTicket
{
    public record AddOwnedTicketRequest(Entities.OwnedTicket Ticket) : IRequest;
}
