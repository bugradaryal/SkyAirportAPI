using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Utilitys.ExceptionHandler;
using Business.Features.Account.Commands.CreateAccount;
using Entities.Enums;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DTO;

namespace Business.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordHandle : IRequestHandler<ChangePasswordRequest, CustomException>
    {
        private UserManager<User> _userManager;
        public ChangePasswordHandle(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CustomException> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.ChangePasswordAsync(request.user, request.changePasswordDTO.OldPassword, request.changePasswordDTO.NewPassword);
                if (!result.Succeeded)
                    return new CustomException(result.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
                return null;
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
