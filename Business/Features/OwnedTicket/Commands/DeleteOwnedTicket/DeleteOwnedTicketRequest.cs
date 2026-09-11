using MediatR;


namespace Business.Features.OwnedTicket.Commands.DeleteOwnedTicket
{
    public record DeleteOwnedTicketRequest(int id) : IRequest;
}
