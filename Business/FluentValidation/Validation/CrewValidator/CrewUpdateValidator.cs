using FluentValidation;
using DTO.Crew;

namespace CrewValidator
{
    public class CrewUpdateValidator : AbstractValidator<CrewUpdateDTO>
    {
        public CrewUpdateValidator()
        {
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

            RuleFor(x => x.aircraft_id)
                .NotEmpty().WithMessage("Aircraft ID is required.");
        }
    }
}
