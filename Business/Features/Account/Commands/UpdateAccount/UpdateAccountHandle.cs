using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Utilitys.ExceptionHandler;
using Business.Features.Account.Commands.DeleteAccount;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.UpdateAccount
{
    public class UpdateAccountHandle : IRequestHandler<UpdateAccountRequest, ResponseModel>
    {
        private UserManager<User> _userManager;
        public UpdateAccountHandle(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseModel> Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                request.user.Uptaded_at = DateTimeOffset.UtcNow;
                var result = await _userManager.UpdateAsync(request.user);
                if (!result.Succeeded)
                    return new ResponseModel { Message = "Cant update account!", Exception = new CustomException(result.Errors.FirstOrDefault().ToString(), 3, (int)HttpStatusCode.BadRequest) };
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
