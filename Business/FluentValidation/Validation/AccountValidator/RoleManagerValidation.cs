using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Account;
using Entities.Enums;
using FluentValidation;

namespace Business.FluentValidation.Validation.AccountValidator
{
    public class RoleManagerValidation : AbstractValidator<RoleManagerDTO>
    {
        public RoleManagerValidation()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.roleName)
                .NotEmpty().WithMessage("RoleName is required.")
                .Must(BeAValidRole).WithMessage("RoleName must be one of the allowed roles.");
        }

        private bool BeAValidRole(string roleName)
        {
            return Enum.GetNames(typeof(Roles)).Contains(roleName);
        }
    }
}
