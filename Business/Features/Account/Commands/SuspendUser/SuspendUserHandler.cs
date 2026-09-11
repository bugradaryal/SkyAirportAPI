using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Commands.ChangePassword;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.Logging.ExceptionHandler;


namespace Business.Features.Account.Commands.SuspendUser
{
    public class SuspendUserHandler : IRequestHandler<SuspendUserRequest>
    {
        private IAccountRepository _accountRepository;
        public SuspendUserHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(SuspendUserRequest request, CancellationToken cancellationToken)
        {
            var suspendResult = await _accountRepository.SuspendUser(request.userId);
            if(!suspendResult)
                throw new CustomException("User not exist!", (int)HttpStatusCode.NotFound);
        }
    }
}
