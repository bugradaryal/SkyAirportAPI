using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Utilitys.Logging.ExceptionHandler;


namespace Business.Features.Account.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserHandler : IRequestHandler<RemoveRoleFromUserRequest>
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RemoveRoleFromUserHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Handle(RemoveRoleFromUserRequest request, CancellationToken cancellationToken)
        {

            var rolDTO = request.roleDTO;
            if (rolDTO.roleName == Roles.User.ToString())
                throw new CustomException("'User' named role cant be deleted!", (int)HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(rolDTO.userId);
            if (user == null)
                throw new CustomException("User not exist!", (int)HttpStatusCode.NotFound);
            if (!await _roleManager.RoleExistsAsync(rolDTO.roleName))
                throw new CustomException("Role not exist!", (int)HttpStatusCode.NotFound);
            var result = await _userManager.RemoveFromRoleAsync(user, rolDTO.roleName);
            if (!result.Succeeded)
                throw new CustomException("Role cant be removed!", (int)HttpStatusCode.BadRequest, result.Errors?.FirstOrDefault()?.ToString());
        }
    }
}
