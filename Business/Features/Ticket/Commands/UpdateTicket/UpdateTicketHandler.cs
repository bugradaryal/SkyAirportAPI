using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Ticket.Commands.AddTicket;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using DataAccess.Concrete;
using MediatR;
using Utilitys.ExceptionHandler;

namespace Business.Features.Ticket.Commands.UpdateTicket
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketRequest,CustomException>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.Ticket> _ticketGenericRepository;
        public UpdateTicketHandler()
        {
            _seatRepository = new SeatRepository();
            _aircraftGenericRepository = new GenericRepository<Entities.Aircraft>();
            _ticketGenericRepository = new GenericRepository<Entities.Ticket>();
            _ticketRepository = new TicketRepository(); 
        }

        public async Task<CustomException> Handle(UpdateTicketRequest request, CancellationToken cancellationToken)
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
                        return new CustomException("Capacity Exceeded!!", (int)HttpStatusCode.BadRequest);
                    aircraft.Current_Capacity = newCapacity;
                    await _aircraftGenericRepository.Update(aircraft);
                }
                await _ticketGenericRepository.Update(ticket);
                return null;
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
