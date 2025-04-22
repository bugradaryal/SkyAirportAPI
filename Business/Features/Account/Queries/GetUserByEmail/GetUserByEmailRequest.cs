using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Account.Queries.GetUserByEmail
{
    public record GetUserByEmailRequest(string email) : IRequest<GetUserByEmailResponse>;
}
