using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.OwnedTicket.Queries
{
    public class GetAllOwnedTicketBySeatIdResponse
    {
        public List<Entities.OwnedTicket>? entity { get; set; }
    }
}
