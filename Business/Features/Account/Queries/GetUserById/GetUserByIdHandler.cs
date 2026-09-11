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


namespace Business.Features.Account.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private UserManager<User> _userManager;
        public GetUserByIdHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);
            if (user == null)
                throw new CustomException("Account doesnt exist!!", (int)HttpStatusCode.NotFound);
            return new GetUserByIdResponse { user = user };
        }
    }
}
