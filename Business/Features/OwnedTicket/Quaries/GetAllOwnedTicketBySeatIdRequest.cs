using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.OwnedTicket.Quaries
{
    public record GetAllOwnedTicketBySeatIdRequest(int id) : IRequest<GetAllOwnedTicketBySeatIdResponse>;
}
