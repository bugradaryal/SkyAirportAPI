using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Business.Features.Account.Queries.GetUserRole
{
    public class GetUserRoleHandler : IRequestHandler<GetUserRoleRequest, GetUserRoleResponse>
    {
        private UserManager<User> _userManager;

        public GetUserRoleHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserRoleResponse> Handle(GetUserRoleRequest request, CancellationToken cancellationToken)
        {
            var roles = await _userManager.GetRolesAsync(new User { Id = request.id });
            return new GetUserRoleResponse { UserRoles = roles.ToList() };
        }
    }
}
