using Business.Features.Seat.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.OwnedTicket.Queries
{
    public record GetAllOwnedTicketBySeatIdRequest(int id) : IRequest<GetAllOwnedTicketBySeatIdResponse>;
}
