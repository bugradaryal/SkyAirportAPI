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

using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.OwnedTicket.Commands.UpdateOwnedTicket
{
    public class UpdateOwnedTicketHandler : IRequestHandler<UpdateOwnedTicketRequest>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IOwnedTicketRepository _ticketRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ticketGenericRepository;
        public UpdateOwnedTicketHandler(ISeatRepository seatRepository, 
            IGenericRepository<Entities.Aircraft> genericAircraftRepository, IGenericRepository<Entities.OwnedTicket>genericOwnedTicketRepository,
            IOwnedTicketRepository ownedTicketRepository)
        {
            _seatRepository = seatRepository;
            _aircraftGenericRepository = genericAircraftRepository;
            _ticketGenericRepository = genericOwnedTicketRepository;
            _ticketRepository = ownedTicketRepository;
        }

        public async Task Handle(UpdateOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            var ticket = request.Ticket;
            if (ticket == null)
                throw new CustomException("Ticket must not null!!", (int)HttpStatusCode.BadRequest);
            if (!await _ticketGenericRepository.Any(ticket.id))
                throw new CustomException("Ticket not found!!", (int)HttpStatusCode.NotFound);
            var oldWeight = await _ticketRepository.GetTicketWeightById(ticket.id);
            if (ticket.Baggage_weight != oldWeight)
            {
                var aircraft = await _seatRepository.GetAircraftByOwnedTicketId(ticket.id);
                var newCapacity = (aircraft.Current_Capacity - oldWeight) + ticket.Baggage_weight;
                if (aircraft.Carry_Capacity < newCapacity)
                    throw new CustomException("Capacity Exceeded!!", (int)HttpStatusCode.BadRequest);
                aircraft.Current_Capacity = newCapacity;
                await _aircraftGenericRepository.Update(aircraft);
            }
            await _ticketGenericRepository.Update(ticket);
        }
    }
}
