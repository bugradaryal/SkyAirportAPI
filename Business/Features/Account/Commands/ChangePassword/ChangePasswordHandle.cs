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
using DTO;

using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordHandle : IRequestHandler<ChangePasswordRequest>
    {
        private UserManager<User> _userManager;
        public ChangePasswordHandle(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userManager.ChangePasswordAsync(request.user, request.changePasswordDTO.OldPassword, request.changePasswordDTO.NewPassword);
            if (!result.Succeeded)
                throw new CustomException ("Changing password not succeded!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());
        }
    }
}
