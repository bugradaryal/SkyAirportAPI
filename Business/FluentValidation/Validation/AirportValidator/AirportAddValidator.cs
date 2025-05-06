using FluentValidation;
using DTO.Airport;

namespace AirportValidator
{
    public class AirportAddValidator : AbstractValidator<AirportAddDTO>
    {
        public AirportAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(128).WithMessage("Name must be at most 128 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(512).WithMessage("Location must be at most 512 characters.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,16}$").WithMessage("Phone number must be valid and between 10 to 16 digits.");

            RuleFor(x => x.MailAdress)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Mail address must be a valid URL.")
                .MaximumLength(96).WithMessage("Mail address must be at most 96 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1024).WithMessage("Description must be at most 1024 characters.");

            RuleFor(x => x.Status)
                .MaximumLength(32).WithMessage("Status must be at most 32 characters.");

        }
    }
}
