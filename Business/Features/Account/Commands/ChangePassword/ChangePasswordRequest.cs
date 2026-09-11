using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MediatR;
using DTO.Account;


namespace Business.Features.Account.Commands.ChangePassword
{
    public record ChangePasswordRequest(User user, ChangePasswordDTO changePasswordDTO) : IRequest;
}
