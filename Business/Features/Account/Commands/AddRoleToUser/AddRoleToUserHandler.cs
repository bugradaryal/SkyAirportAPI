using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Commands.ChangePassword;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.AddRoleToUser
{
    public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserRequest, ResponseModel>
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleToUserHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel> Handle(AddRoleToUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var rolDTO = request.roleDTO;
                var user = await _userManager.FindByIdAsync(rolDTO.userId);
                if (user != null)
                {
                    if (!await _roleManager.RoleExistsAsync(rolDTO.roleName))
                        return new ResponseModel { Message = "Role not exist!" };
                    var result = await _userManager.AddToRoleAsync(user, rolDTO.roleName);
                    if (!result.Succeeded)
                        return new ResponseModel { Message = "Role cant be added!", Exception = new CustomException(result.Errors.FirstOrDefault().ToString(), 3, (int)HttpStatusCode.BadRequest) };
                    return null;
                }
                return new ResponseModel { Message = "User not exist!" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
