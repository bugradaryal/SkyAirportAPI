using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Seat.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;

namespace Business.Features.Ticket.Quaries
{
    public class GetAllTicketBySeatIdHandler : IRequestHandler<GetAllTicketBySeatIdRequest, GetAllTicketBySeatIdResponse>
    {
        private readonly ITicketRepository _ticketRepository;
        public GetAllTicketBySeatIdHandler()
        {
            _ticketRepository = new TicketRepository();
        }

        public async Task<GetAllTicketBySeatIdResponse> Handle(GetAllTicketBySeatIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _ticketRepository.GetAllBySeatId(request.id);
                return new GetAllTicketBySeatIdResponse { entity = ticket, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllTicketBySeatIdResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) };
            }
        }
    }
}
