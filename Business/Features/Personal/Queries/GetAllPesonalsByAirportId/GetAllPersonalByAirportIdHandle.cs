using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Business.Features.Account.Commands.ChangePassword;
using Business.Features.Personal.Queries.GetAllPesonalsByAirportId;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Personal.Commands.GetAllPesonalsByAirportId
{
    public class GetAllPersonalByAirportIdHandle : IRequestHandler<GetAllPersonalByAirportIdRequest, GetAllPersonalByAirportIdResponse>
    {
        private readonly IPersonalRepository _personalRepository;
        public GetAllPersonalByAirportIdHandle()
        {
            _personalRepository = new PersonalRepository();
        }

        public async Task<GetAllPersonalByAirportIdResponse> Handle(GetAllPersonalByAirportIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var personals = await _personalRepository.GetAllByAirportId(request.id);
                return new GetAllPersonalByAirportIdResponse { entity = personals , error = false };
            }
            catch (Exception ex)
            {
                return new GetAllPersonalByAirportIdResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
