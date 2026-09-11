using DTO.Account;
using MediatR;


namespace Business.Features.Account.Commands.AddRoleToUser
{
    public record AddRoleToUserRequest(RoleManagerDTO roleDTO) : IRequest;
}
