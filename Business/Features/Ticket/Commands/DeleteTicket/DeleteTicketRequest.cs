using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Utilitys.ExceptionHandler;

namespace Business.Features.Ticket.Commands.DeleteTicket
{
    public record DeleteTicketRequest(int id) : IRequest<CustomException>;
}
