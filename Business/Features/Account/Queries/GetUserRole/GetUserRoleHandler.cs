using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static Entities.Enums.Default_Authorization_Type;
using Business.Features.Account.Queries.GetUserByEmail;
using Utilitys.ResponseHandler;

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
                return new GetUserRoleResponse { UserRoles = roles.ToList(), error = false }; 
            }
            catch (Exception ex)
            {
                return new GetUserRoleResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
