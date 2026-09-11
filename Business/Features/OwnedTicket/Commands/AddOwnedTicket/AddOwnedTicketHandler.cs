using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Commands.CreateAccount;
using DTO.Account;
using Entities.Enums;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.Generic;

using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.OwnedTicket.Commands.AddOwnedTicket
{
    public class AddOwnedTicketHandler : IRequestHandler<AddOwnedTicketRequest>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ownedticketGenericRepository;
        public AddOwnedTicketHandler(ISeatRepository seatRepository, IGenericRepository<Entities.Aircraft> genericAircraftRepository,
            IGenericRepository<Entities.OwnedTicket> genericOwnedTicketRepository)
        {
            _seatRepository = seatRepository;
            _aircraftGenericRepository = genericAircraftRepository;
            _ownedticketGenericRepository = genericOwnedTicketRepository;
        }

        public async Task Handle(AddOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            var ticket = request.Ticket;
            if(ticket == null)
                throw new CustomException("Ticket must not null!!", (int)HttpStatusCode.BadRequest);
            if (!await _seatRepository.IsSeatAvailable(ticket.id) == true)
                throw new CustomException("Seat allready puchased!!", (int)HttpStatusCode.BadRequest);

            var aircraft = await _seatRepository.GetAircraftByOwnedTicketId(ticket.id);
            var sumCapacity = aircraft.Current_Capacity + ticket.Baggage_weight;
            if (aircraft.Carry_Capacity < sumCapacity)
                throw new CustomException("Capacity Exceeded!!", (int)HttpStatusCode.BadRequest);
            aircraft.Current_Capacity = sumCapacity;
            await _aircraftGenericRepository.Update(aircraft);
            await _ownedticketGenericRepository.Add(ticket);
            await _seatRepository.SetSeatAvailable(ticket.ticket_id, false);
        }
    }
}
