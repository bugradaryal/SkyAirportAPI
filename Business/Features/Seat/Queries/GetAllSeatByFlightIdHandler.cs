using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Airline.Queries;
using DataAccess.Abstract;
using DataAccess.Concrete;
using MediatR;

namespace Business.Features.Seat.Queries
{
    public class GetAllSeatByFlightIdHandler : IRequestHandler<GetAllSeatByFlightIdRequest,GetAllSeatByFlightIdResponse>
    {
        private readonly ISeatRepository _seatRepository;
        public GetAllSeatByFlightIdHandler()
        {
            _seatRepository = new SeatRepository();
        }

        public async Task<GetAllSeatByFlightIdResponse> Handle(GetAllSeatByFlightIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var seats = await _seatRepository.GetAllByFlightId(request.id);
                return new GetAllSeatByFlightIdResponse { entity = seats, error = false };
            }
            catch (Exception ex)
            {
                return new GetAllSeatByFlightIdResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
