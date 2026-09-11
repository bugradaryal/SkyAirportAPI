using MediatR;

namespace Business.Features.Account.Queries.GetUserRole
{
    public record GetUserRoleRequest(string id) : IRequest<GetUserRoleResponse>;
}
