using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Queries.GetUserRole;
using DTO;
using MediatR;

namespace Business.Features.Account.Queries.Login
{
    public record LoginRequest(LoginAccountDTO loginAccountDTO) : IRequest<LoginResponse>;
}
