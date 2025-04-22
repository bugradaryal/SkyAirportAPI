using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Business.Features.Account.Commands.UpdateAccount;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Account.Queries.GetUserByEmail
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailRequest, GetUserByEmailResponse>
    {
        private UserManager<User> _userManager;
        public GetUserByEmailHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserByEmailResponse> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(request.email);
                if (result == null)
                    return new GetUserByEmailResponse { exception = new CustomException("Account doesnt exist!",(int)HttpStatusCode.NotFound) };
                return new GetUserByEmailResponse { user = result, error = false };
            }
            catch (Exception ex)
            {
                return new GetUserByEmailResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
