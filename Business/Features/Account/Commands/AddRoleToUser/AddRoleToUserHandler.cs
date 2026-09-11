using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Utilitys.Logging.ExceptionHandler;


namespace Business.Features.Account.Commands.AddRoleToUser
{
    public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserRequest>
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleToUserHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Handle(AddRoleToUserRequest request, CancellationToken cancellationToken)
        {
            var rolDTO = request.roleDTO;
            var user = await _userManager.FindByIdAsync(rolDTO.userId);
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(rolDTO.roleName))
                    throw new CustomException("Role not exist!", (int)HttpStatusCode.NotFound);
                var result = await _userManager.AddToRoleAsync(user, rolDTO.roleName);
                if (!result.Succeeded)
                    throw new CustomException("Role cant be added!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());
            }
            else
                throw new CustomException("User not exist!", (int)HttpStatusCode.NotFound);
        }
    }
}
