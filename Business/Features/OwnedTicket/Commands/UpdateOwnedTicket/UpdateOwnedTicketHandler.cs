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

namespace Business.Features.OwnedTicket.Commands.UpdateOwnedTicket
{
    public class UpdateOwnedTicketHandler : IRequestHandler<UpdateOwnedTicketRequest, ResponseModel>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IOwnedTicketRepository _ticketRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ticketGenericRepository;
        public UpdateOwnedTicketHandler()
        {
            _seatRepository = new SeatRepository();
            _aircraftGenericRepository = new GenericRepository<Entities.Aircraft>();
            _ticketGenericRepository = new GenericRepository<Entities.OwnedTicket>();
            _ticketRepository = new OwnedTicketRepository(); 
        }

        public async Task<ResponseModel> Handle(UpdateOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = request.Ticket;
                var oldWeight = await _ticketRepository.GetTicketWeightById(ticket.id);
                if(ticket.Baggage_weight != oldWeight)
                {
                    var aircraft = await _seatRepository.GetSeatAndAircraftByTicketId(ticket.id);
                    var newCapacity = (aircraft.Current_Capacity - oldWeight) + ticket.Baggage_weight;
                    if (aircraft.Carry_Capacity < newCapacity)
                        return new ResponseModel { Message = "Capacity Exceeded!!" };
                    aircraft.Current_Capacity = newCapacity;
                    await _aircraftGenericRepository.Update(aircraft);
                }
                await _ticketGenericRepository.Update(ticket);
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
