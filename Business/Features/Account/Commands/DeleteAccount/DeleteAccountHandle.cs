using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Features.Account.Commands.CreateAccount;
using Entities.Enums;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.Account.Commands.DeleteAccount
{
    public class DeleteAccountHandle : IRequestHandler<DeleteAccountRequest>
    {
        private UserManager<User> _userManager;
        public DeleteAccountHandle(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
        {

            var result = await _userManager.DeleteAsync(request.user);
            if (!result.Succeeded)
                throw new CustomException("Cant delete account!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());
        }
    }
}
