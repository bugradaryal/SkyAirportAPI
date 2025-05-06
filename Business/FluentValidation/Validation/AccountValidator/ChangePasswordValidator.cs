using FluentValidation;
using DTO.Account;

namespace AccountValidator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.")
                .Length(6, 16).WithMessage("Old password must be between 6 and 16 characters.")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])").WithMessage("Password must contain at least one uppercase letter and one lowercase letter.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .Length(6, 16).WithMessage("New password must be between 6 and 16 characters.")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])").WithMessage("Password must contain at least one uppercase letter and one lowercase letter.");


        }
    }
}
