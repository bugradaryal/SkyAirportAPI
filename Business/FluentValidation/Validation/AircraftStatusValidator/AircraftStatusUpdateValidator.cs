using FluentValidation;
using DTO.AircraftStatus;

namespace AircraftStatusValidator
{
    public class AircraftStatusUpdateValidator : AbstractValidator<AircraftStatusUpdateDTO>
    {
        public AircraftStatusUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotNull().WithMessage("ID is required.");

            RuleFor(x => x.Status)
                .MaximumLength(64).WithMessage("Status must be at most 64 characters.");

        }
    }
}
