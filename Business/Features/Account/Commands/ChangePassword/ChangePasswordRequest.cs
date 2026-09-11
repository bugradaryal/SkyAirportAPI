using DTO.Account;
using Entities;
using MediatR;


namespace Business.Features.Account.Commands.ChangePassword
{
    public record ChangePasswordRequest(User user, ChangePasswordDTO changePasswordDTO) : IRequest;
}
