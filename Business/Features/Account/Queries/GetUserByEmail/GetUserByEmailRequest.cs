using MediatR;

namespace Business.Features.Account.Queries.GetUserByEmail
{
    public record GetUserByEmailRequest(string email) : IRequest<GetUserByEmailResponse>;
}
