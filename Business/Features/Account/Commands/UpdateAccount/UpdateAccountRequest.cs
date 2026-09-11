using DTO.Account;
using MediatR;


namespace Business.Features.Account.Commands.UpdateAccount
{
    public record UpdateAccountRequest(UpdateAccountDTO updateAccountDTO, string userId) : IRequest;
}
