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

namespace Business.Features.OwnedTicket.Commands.DeleteOwnedTicket
{
    public class DeleteOwnedTicketHandler : IRequestHandler<DeleteOwnedTicketRequest>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ticketGenericRepository;
        public DeleteOwnedTicketHandler(ISeatRepository seatRepository, IGenericRepository<Entities.Aircraft> genericAircraftRepository,
            IGenericRepository<Entities.OwnedTicket> genericOwnedTicketRepository)
        {
            _seatRepository = seatRepository;
            _aircraftGenericRepository = genericAircraftRepository;
            _ticketGenericRepository = genericOwnedTicketRepository;
        }

        public async Task Handle(DeleteOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketGenericRepository.GetValue(request.id);
            if (ticket == null)
                throw new CustomException("Ticket not found!!", (int)HttpStatusCode.NotFound);
            if (!await _ticketGenericRepository.Any(ticket.id))
                throw new CustomException("Ticket not found!!", (int)HttpStatusCode.NotFound);
            var aircraft = await _seatRepository.GetAircraftByOwnedTicketId(ticket.id);
            var sumCapacity = aircraft.Current_Capacity - ticket.Baggage_weight;
            if (sumCapacity < 0)
                sumCapacity = 0;

            aircraft.Current_Capacity = sumCapacity;
            await _aircraftGenericRepository.Update(aircraft);
            await _ticketGenericRepository.Delete(ticket.id);
            await _seatRepository.SetSeatAvailable(ticket.ticket_id, true);
        }
    }
}
