using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.ExceptionHandler;
using Business.Features.Account.Commands.DeleteAccount;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Account.Commands.UpdateAccount
{
    public class UpdateAccountHeader : IRequestHandler<UpdateAccountRequest, CustomException>
    {
        private UserManager<User> _userManager;
        public UpdateAccountHeader(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CustomException> Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.UpdateAsync(request.user);
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
