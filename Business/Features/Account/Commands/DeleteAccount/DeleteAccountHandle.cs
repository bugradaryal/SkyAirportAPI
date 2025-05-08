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
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.DeleteAccount
{
    public class DeleteAccountHandle : IRequestHandler<DeleteAccountRequest, ResponseModel>
    {
        private UserManager<User> _userManager;
        public DeleteAccountHandle(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseModel> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.DeleteAsync(request.user);
                if (!result.Succeeded)
                    return new ResponseModel { Message = "Cant delete account!", Exception = new CustomException(result.Errors.FirstOrDefault().ToString(), 3, (int)HttpStatusCode.BadRequest) };
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
