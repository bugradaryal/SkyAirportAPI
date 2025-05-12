using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Account;
using MediatR;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Commands.RemoveRoleFromUser
{
    public record RemoveRoleFromUserRequest(RoleManagerDTO roleDTO) : IRequest<ResponseModel>;
}
