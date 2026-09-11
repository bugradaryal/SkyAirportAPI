using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.Account;
using Entities;
using MediatR;


namespace Business.Features.Account.Commands.UpdateAccount
{
    public record UpdateAccountRequest(UpdateAccountDTO updateAccountDTO, string userId) : IRequest;
}
