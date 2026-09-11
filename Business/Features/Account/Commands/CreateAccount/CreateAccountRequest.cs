using DTO.Account;
using MediatR;


namespace Business.Features.Account.Commands.CreateAccount
{
    public record CreateAccountRequest(CreateAccountDTO createAccountDTO) : IRequest;
}
