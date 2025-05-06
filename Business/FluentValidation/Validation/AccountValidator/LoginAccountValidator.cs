using FluentValidation;
using DTO.Account;

namespace AccountValidator
{
    public class LoginAccountValidator : AbstractValidator<LoginAccountDTO>
    {
        public LoginAccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .Length(6, 255).WithMessage("Email must be between 6 and 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 16).WithMessage("Password must be between 6 and 16 characters.");

        }
    }
}
