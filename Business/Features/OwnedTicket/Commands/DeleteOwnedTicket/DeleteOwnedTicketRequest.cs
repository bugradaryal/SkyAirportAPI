using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.OwnedTicket.Commands.DeleteOwnedTicket
{
    public record DeleteOwnedTicketRequest(int id) : IRequest<ResponseModel>;
}
