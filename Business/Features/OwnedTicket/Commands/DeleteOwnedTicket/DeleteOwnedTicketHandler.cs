using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using DataAccess.Concrete;
using MediatR;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.OwnedTicket.Commands.DeleteOwnedTicket
{
    public class DeleteOwnedTicketHandler : IRequestHandler<DeleteOwnedTicketRequest, ResponseModel>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ticketGenericRepository;
        public DeleteOwnedTicketHandler()
        {
            _seatRepository = new SeatRepository();
            _aircraftGenericRepository = new GenericRepository<Entities.Aircraft>();
            _ticketGenericRepository = new GenericRepository<Entities.OwnedTicket>();
        }

        public async Task<ResponseModel> Handle(DeleteOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _ticketGenericRepository.GetValue(request.id);
                var aircraft = await _seatRepository.GetSeatAndAircraftByTicketId(ticket.id);
                var sumCapacity = aircraft.Current_Capacity - ticket.Baggage_weight;
                if (sumCapacity < 0)
                    sumCapacity = 0;

                aircraft.Current_Capacity = sumCapacity;
                await _aircraftGenericRepository.Update(aircraft);
                await _ticketGenericRepository.Delete(ticket.id);
                await _seatRepository.SetSeatAvailable(ticket.ticket_id, true);
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
