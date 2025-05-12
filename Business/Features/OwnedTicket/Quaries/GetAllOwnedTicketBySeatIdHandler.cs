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
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Utilitys.ResponseHandler;

namespace Business.Features.OwnedTicket.Quaries
{
    public class GetAllOwnedTicketBySeatIdHandler : IRequestHandler<GetAllOwnedTicketBySeatIdRequest, GetAllOwnedTicketBySeatIdResponse>
    {
        private readonly IOwnedTicketRepository _ticketRepository;
        public GetAllOwnedTicketBySeatIdHandler()
        {
            _ticketRepository = new OwnedTicketRepository();
        }

        public async Task<GetAllOwnedTicketBySeatIdResponse> Handle(GetAllOwnedTicketBySeatIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _ticketRepository.GetAllBySeatId(request.id);
                return new GetAllOwnedTicketBySeatIdResponse { entity = ticket, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllOwnedTicketBySeatIdResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
