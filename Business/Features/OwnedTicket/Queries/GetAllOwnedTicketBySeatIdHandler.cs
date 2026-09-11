using DataAccess.Abstract;
using MediatR;

namespace Business.Features.OwnedTicket.Queries
{
    public class GetAllOwnedTicketBySeatIdHandler : IRequestHandler<GetAllOwnedTicketBySeatIdRequest, GetAllOwnedTicketBySeatIdResponse>
    {
        private readonly IOwnedTicketRepository _ownedRepository;
        public GetAllOwnedTicketBySeatIdHandler(IOwnedTicketRepository ownedTicket)
        {
            _ownedRepository = ownedTicket;
        }

        public async Task<GetAllOwnedTicketBySeatIdResponse> Handle(GetAllOwnedTicketBySeatIdRequest request, CancellationToken cancellationToken)
        {
            var ownedTicket = await _ownedRepository.GetTicketBySeatId(request.id);
            return new GetAllOwnedTicketBySeatIdResponse { entity = ownedTicket };
        }
    }
}
