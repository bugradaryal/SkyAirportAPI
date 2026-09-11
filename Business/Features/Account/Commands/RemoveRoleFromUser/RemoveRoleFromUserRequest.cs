using DTO.Account;
using MediatR;


namespace Business.Features.Account.Commands.RemoveRoleFromUser
{
    public record RemoveRoleFromUserRequest(RoleManagerDTO roleDTO) : IRequest;
}
