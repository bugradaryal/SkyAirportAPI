using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using DTO;
using MediatR;

namespace Business.Features.Account.Commands.CreateAccount
{
    public record CreateAccountRequest(CreateAccountDTO createAccountDTO) : IRequest<CustomException>;
}
