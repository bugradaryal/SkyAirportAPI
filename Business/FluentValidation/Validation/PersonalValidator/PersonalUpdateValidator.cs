using FluentValidation;
using DTO.Personal;

namespace PersonalValidator
{
    public class PersonalUpdateValidator : AbstractValidator<PersonalUpdateDTO>
    {
        public PersonalUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Name)
                .MaximumLength(64).WithMessage("Name cannot exceed 64 characters.");

            RuleFor(x => x.Surname)
                .MaximumLength(64).WithMessage("Surname cannot exceed 64 characters.");

            RuleFor(x => x.Role)
                .MaximumLength(256).WithMessage("Role cannot exceed 256 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 120).WithMessage("Age must be between 18 and 120.");

            RuleFor(x => x.Gender)
                   .Must(gender => gender == 'E' || gender == 'K' || gender == 'U').WithMessage("Gender must be 'E', 'K', or 'U'.");

            RuleFor(x => x.PhoneNumber)
                .Length(10, 16).WithMessage("Phone number must be between 10 and 16 characters.");

            RuleFor(x => x.Start_Date)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.airport_id)
                .NotEmpty().WithMessage("Airport ID is required.");

        }
    }
}
