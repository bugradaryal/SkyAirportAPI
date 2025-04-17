using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static Entities.Enums.Default_Authorization_Type;

namespace Business.Features.Account.Queries.GetUserRole
{
    public class GetUserRoleHandler : IRequestHandler<GetUserRoleRequest, GetUserRoleResponse>
    {
        private UserManager<User> _userManager;

        public GetUserRoleHandler(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<GetUserRoleResponse> Handle(GetUserRoleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(new User { Id = request.id });
                return new GetUserRoleResponse { UserRoles = roles.ToList() };  // İstenirse ToList() ile dönüştürülür, fakat IList de çalışır
            }
            catch (Exception ex)
            {
                return new GetUserRoleResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
