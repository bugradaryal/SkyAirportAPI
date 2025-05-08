using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Ticket.Quaries
{
    public class GetAllTicketBySeatIdResponse
    {
        public List<Entities.Ticket> entity { get; set; }
        public ResponseModel response { get; set; }
        public bool error { get; set; } = true;

    }
}
