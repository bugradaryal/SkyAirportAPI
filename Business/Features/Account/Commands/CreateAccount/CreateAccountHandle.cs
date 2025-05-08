using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.Mapper;
using DTO.Account;
using Utilitys;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.CreateAccount
{
    public class CreateAccountHandle : IRequestHandler<CreateAccountRequest, ResponseModel>
    {
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        public CreateAccountHandle(IMapper mapper, UserManager<User> userManager)
        {
            this._mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResponseModel> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                User user = _mapper.Map<User,CreateAccountDTO>(request.createAccountDTO);
                string password = request.createAccountDTO.Password;
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return new ResponseModel { Message = "Cant create account!", Exception = new CustomException(result.Errors.FirstOrDefault().ToString(), 3, (int)HttpStatusCode.BadRequest) };
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Default_Authorization_Type.default_role.ToString());
                    if (!roleResult.Succeeded)
                        return new ResponseModel { Message = "Cant set user role to default!", Exception = new CustomException(roleResult.Errors.FirstOrDefault().ToString(), 3, (int)HttpStatusCode.BadRequest) };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
