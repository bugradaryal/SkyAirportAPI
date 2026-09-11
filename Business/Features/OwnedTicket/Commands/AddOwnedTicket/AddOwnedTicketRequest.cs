using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace Business.Features.OwnedTicket.Commands.AddOwnedTicket
{
    public record AddOwnedTicketRequest(Entities.OwnedTicket Ticket) : IRequest;
}
