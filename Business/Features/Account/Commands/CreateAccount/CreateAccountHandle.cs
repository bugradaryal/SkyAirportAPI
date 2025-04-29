using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using DTO;
using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.Mapper;

namespace Business.Features.Account.Commands.CreateAccount
{
    public class CreateAccountHandle : IRequestHandler<CreateAccountRequest, CustomException>
    {
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        public CreateAccountHandle(IMapper mapper, UserManager<User> userManager)
        {
            this._mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CustomException> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                User user = _mapper.Map<User,CreateAccountDTO>(request.createAccountDTO);
                string password = request.createAccountDTO.Password;
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return new CustomException(result.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Default_Authorization_Type.default_role.ToString());
                    if (!roleResult.Succeeded)
                        return new CustomException(roleResult.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
                }
                return null;
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
