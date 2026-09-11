using DTO.Account;
using MediatR;

namespace Business.Features.Account.Queries.Login
{
    public record LoginRequest(LoginAccountDTO loginAccountDTO) : IRequest<LoginResponse>;
}
