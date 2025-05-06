using FluentValidation;
using DTO.Crew;

namespace CrewValidator
{
    public class CrewAddValidator : AbstractValidator<CrewAddDTO>
    {
        public CrewAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name must be at most 64 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(64).WithMessage("Surname must be at most 64 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(256).WithMessage("Role must be at most 256 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 120).WithMessage("Age must be between 18 and 120.");

            RuleFor(x => x.Gender)
                .Must(gender => gender == 'E' || gender == 'K' || gender == 'U').WithMessage("Gender must be 'E', 'K', or 'U'.");

            RuleFor(x => x.aircraft_id)
                .GreaterThan(0).WithMessage("Aircraft ID must be greater than 0.");

        }
    }
}
