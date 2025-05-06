using FluentValidation;
using DTO.Personal;

namespace PersonalValidator
{
    public class PersonalAddValidator : AbstractValidator<PersonalAddDTO>
    {
        public PersonalAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot exceed 64 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(64).WithMessage("Surname cannot exceed 64 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(256).WithMessage("Role cannot exceed 256 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 120).WithMessage("Age must be between 18 and 120.");

            RuleFor(x => x.Gender)
                   .Must(gender => gender == 'E' || gender == 'K' || gender == 'U').WithMessage("Gender must be 'E', 'K', or 'U'.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Length(10, 16).WithMessage("Phone number must be between 10 and 16 characters.");

            RuleFor(x => x.Start_Date)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.airport_id)
                .NotEmpty().WithMessage("Airport ID is required.");

        }
    }
}
