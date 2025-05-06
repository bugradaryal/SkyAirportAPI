using FluentValidation;
using DTO.Account;

namespace AccountValidator
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountDTO>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 64).WithMessage("Name must be between 2 and 64 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 64).WithMessage("Surname must be between 2 and 64 characters.");

            RuleFor(x => x.Gender)
                .Must(gender => gender == 'E' || gender == 'K' || gender == 'U').WithMessage("Gender must be 'E', 'K', or 'U'.");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .InclusiveBetween(1, 120).WithMessage("Age must be between 1 and 120.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .Length(6, 255).WithMessage("Email must be between 6 and 255 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,16}$").WithMessage("Phone number must be valid and between 10 to 16 digits.");

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Matches(@"^\+[0-9]{1,4}$").WithMessage("Invalid country code format.")
                .Length(2, 5).WithMessage("Country code must be between 2 and 5 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(5, 32).WithMessage("Username must be between 5 and 32 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 16).WithMessage("Password must be between 6 and 16 characters.");

        }
    }
}
