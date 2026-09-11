using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.Mapper;
using DTO.Account;
using Utilitys;

using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.Account.Commands.CreateAccount
{
    public class CreateAccountHandle : IRequestHandler<CreateAccountRequest>
    {
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        public CreateAccountHandle(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            User user = _mapper.Map<User, CreateAccountDTO>(request.createAccountDTO);
            string password = request.createAccountDTO.Password;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new CustomException("Cant create account!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());

            var roleResult = await _userManager.AddToRoleAsync(user, Default_Authorization_Type.default_role.ToString());
            if (!roleResult.Succeeded)
                throw new CustomException("Cant set user role to default!", (int)HttpStatusCode.BadRequest, roleResult.Errors.FirstOrDefault().ToString());
        }
    }
}
