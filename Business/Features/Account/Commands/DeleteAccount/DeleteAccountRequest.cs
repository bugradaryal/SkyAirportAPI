using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Entities;
using MediatR;


namespace Business.Features.Account.Commands.DeleteAccount
{
    public record DeleteAccountRequest(User user) : IRequest;
}
