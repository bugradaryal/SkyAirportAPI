using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using DTO;
using Entities; 
using MediatR;

namespace Business.Features.Account.Commands.ChangePassword
{
    public record ChangePasswordRequest(User user, ChangePasswordDTO changePasswordDTO) : IRequest<CustomException>;
}
