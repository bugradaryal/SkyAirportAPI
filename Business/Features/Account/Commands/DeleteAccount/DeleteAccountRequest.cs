using Entities;
using MediatR;


namespace Business.Features.Account.Commands.DeleteAccount
{
    public record DeleteAccountRequest(User user) : IRequest;
}
