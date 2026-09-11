using DTO.Account;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Utilitys.Logging.ExceptionHandler;
using Utilitys.Mapper;


namespace Business.Features.Account.Commands.UpdateAccount
{
    public class UpdateAccountHandle : IRequestHandler<UpdateAccountRequest>
    {
        private UserManager<User> _userManager;
        private IMapper _mapper;
        public UpdateAccountHandle(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            var userResponse = await _userManager.FindByIdAsync(request.userId);
            User user = _mapper.Map<User, UpdateAccountDTO>(request.updateAccountDTO, userResponse);
            user.Uptaded_at = DateTimeOffset.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new CustomException("Cant update account!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());
        }
    }
}
