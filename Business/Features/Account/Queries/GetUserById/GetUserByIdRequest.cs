using MediatR;

namespace Business.Features.Account.Queries.GetUserById
{
    public record GetUserByIdRequest(string userId) : IRequest<GetUserByIdResponse>;
}
