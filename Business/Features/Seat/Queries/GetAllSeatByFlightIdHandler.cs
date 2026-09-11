using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Airline.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;
using Business.Features.Aircraft.Queries.GetAllAircrafts;


namespace Business.Features.Seat.Queries
{
    public class GetAllSeatByFlightIdHandler : IRequestHandler<GetAllSeatTicketRequest,GetAllSeatTicketResponse>
    {
        private readonly ISeatRepository _seatRepository;
        public GetAllSeatByFlightIdHandler(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<GetAllSeatTicketResponse> Handle(GetAllSeatTicketRequest request, CancellationToken cancellationToken)
        {
            var seats = await _seatRepository.GetAllByFlightId(request.id);
            return new GetAllSeatTicketResponse { entity = seats };
        }
    }
}
