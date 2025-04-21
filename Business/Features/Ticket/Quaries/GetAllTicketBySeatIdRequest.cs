using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Ticket.Quaries
{
    public record GetAllTicketBySeatIdRequest(int id) : IRequest<GetAllTicketBySeatIdResponse>;
}
