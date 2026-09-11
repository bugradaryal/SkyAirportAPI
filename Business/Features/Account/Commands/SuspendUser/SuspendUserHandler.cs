using DataAccess.Abstract;
using MediatR;
using System.Net;
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
            if (!suspendResult)
                throw new CustomException("User not exist!", (int)HttpStatusCode.NotFound);
        }
    }
}
