using FluentValidation;
using DTO.Airline;

namespace AirlineValidator
{
    public class AirlineAddValidator : AbstractValidator<AirlineAddDTO>
    {
        public AirlineAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(128).WithMessage("Name must be at most 128 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1024).WithMessage("Description must be at most 1024 characters.");

            RuleFor(x => x.WebAdress)
                .NotEmpty().WithMessage("Web address is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Web address must be a valid URL.")
                .MaximumLength(128).WithMessage("Web address must be at most 128 characters.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,16}$").WithMessage("Phone number must be valid and between 10 to 16 digits.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(60).WithMessage("Country must be at most 60 characters.");

            RuleFor(x => x.airport_id)
                .NotNull().WithMessage("Airport ID is required.");

        }
    }
}
