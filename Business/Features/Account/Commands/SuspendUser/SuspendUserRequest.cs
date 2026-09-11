using MediatR;


namespace Business.Features.Account.Commands.SuspendUser
{
    public record SuspendUserRequest(string userId) : IRequest;
}
