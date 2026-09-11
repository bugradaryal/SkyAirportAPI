using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Commands.UpdateAccount;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.Logging.ExceptionHandler;


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
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user == null)
                throw new CustomException("Account doesnt exist!!", (int)HttpStatusCode.NotFound);
            return new GetUserByEmailResponse { user = user };
        }
    }
}
