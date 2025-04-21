using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;

namespace Business.Features.Ticket.Quaries
{
    public class GetAllTicketBySeatIdResponse
    {
        public List<Entities.Ticket> entity { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;

    }
}
