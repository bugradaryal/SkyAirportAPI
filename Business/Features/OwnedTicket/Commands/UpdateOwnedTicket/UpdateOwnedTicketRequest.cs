using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.OwnedTicket.Commands.UpdateOwnedTicket
{
    public record UpdateOwnedTicketRequest(Entities.OwnedTicket Ticket) : IRequest<ResponseModel>;
}
