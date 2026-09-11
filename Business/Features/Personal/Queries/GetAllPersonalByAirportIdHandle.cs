using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Personal.Queries
{
    public class GetAllPersonalByAirportIdHandle : IRequestHandler<GetAllPersonalByAirportIdRequest, GetAllPersonalByAirportIdResponse>
    {
        private readonly IPersonalRepository _personalRepository;
        public GetAllPersonalByAirportIdHandle(IPersonalRepository personalRepository)
        {
            _personalRepository = personalRepository;
        }

        public async Task<GetAllPersonalByAirportIdResponse> Handle(GetAllPersonalByAirportIdRequest request, CancellationToken cancellationToken)
        {
            var personals = await _personalRepository.GetAllByAirportId(request.id);
            return new GetAllPersonalByAirportIdResponse { entity = personals };
        }
    }
}
