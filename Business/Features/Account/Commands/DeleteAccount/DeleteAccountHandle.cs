using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
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
