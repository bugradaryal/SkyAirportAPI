using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using DTO;
using Entities;
using MediatR;

namespace Business.Features.Account.Commands.UpdateAccount
{
    public record UpdateAccountRequest(User user) : IRequest<CustomException>;
}
