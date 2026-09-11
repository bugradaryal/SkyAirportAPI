using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Seat.Queries
{
    public class GetAllSeatByFlightIdHandler : IRequestHandler<GetAllSeatTicketRequest, GetAllSeatTicketResponse>
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
