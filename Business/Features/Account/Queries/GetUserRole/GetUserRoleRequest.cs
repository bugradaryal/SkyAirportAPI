using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Account.Queries.GetUserRole
{
    public record GetUserRoleRequest(string id) : IRequest<GetUserRoleResponse>;
}
