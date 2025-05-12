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
using Utilitys.ExceptionHandler;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.Generic;
using Utilitys.ResponseHandler;

namespace Business.Features.OwnedTicket.Commands.AddOwnedTicket
{
    public class AddOwnedTicketHandler : IRequestHandler<AddOwnedTicketRequest,ResponseModel>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IGenericRepository<Entities.Aircraft> _aircraftGenericRepository;
        private readonly IGenericRepository<Entities.OwnedTicket> _ticketGenericRepository;
        public AddOwnedTicketHandler()
        {
            _seatRepository = new SeatRepository();
            _aircraftGenericRepository = new GenericRepository<Entities.Aircraft>();
            _ticketGenericRepository = new GenericRepository<Entities.OwnedTicket>();
        }

        public async Task<ResponseModel> Handle(AddOwnedTicketRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = request.Ticket;
                if(await _seatRepository.IsSeatAvailable(ticket.id) == true)
                {
                    var aircraft = await _seatRepository.GetSeatAndAircraftByTicketId(ticket.id);
                    var sumCapacity = aircraft.Current_Capacity + ticket.Baggage_weight;
                    if (aircraft.Carry_Capacity < sumCapacity)
                        return new ResponseModel { Message = "Capacity Exceeded!" };
                    aircraft.Current_Capacity = sumCapacity;
                    await _aircraftGenericRepository.Update(aircraft);
                    await _ticketGenericRepository.Add(ticket);
                    await _seatRepository.SetSeatAvailable(ticket.seat_id, false);
                    return null;
                }
                return new ResponseModel { Message = "Seat allready puchased!!" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
