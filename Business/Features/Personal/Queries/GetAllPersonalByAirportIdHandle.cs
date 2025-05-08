using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Account.Commands.ChangePassword;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Business.Features.Aircraft.Queries.GetAllAircrafts;

namespace Business.Features.Personal.Queries
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
                return new GetAllPersonalByAirportIdResponse { response = { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
