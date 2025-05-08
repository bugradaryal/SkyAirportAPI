using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Ticket.Commands.AddTicket
{
    public record AddTicketRequest(Entities.Ticket Ticket) : IRequest<ResponseModel>;
}
