using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Commands.ChangePassword;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.SuspendUser
{
    public class SuspendUserHandler : IRequestHandler<SuspendUserRequest, ResponseModel>
    {
        private IAccountRepository _accountRepository;
        public SuspendUserHandler()
        {
            _accountRepository = new AccountRepository();
        }

        public async Task<ResponseModel> Handle(SuspendUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _accountRepository.SuspendUser(request.userId);
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
