using FluentValidation;
using DTO.Aircraft;

namespace AircraftValidator
{
    public class AircraftUpdateValidator : AbstractValidator<AircraftUpdateDTO>
    {
        public AircraftUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotNull().WithMessage("ID is required.");

            RuleFor(x => x.Model)
                .MaximumLength(64).WithMessage("Model must be at most 64 characters.");

            RuleFor(x => x.Fuel_Capacity)
                .InclusiveBetween(0, 999999.9m).WithMessage("Fuel capacity must be between 0 and 999999.9.");

            RuleFor(x => x.Max_Altitude)
                .InclusiveBetween(0, 999999.9m).WithMessage("Max altitude must be between 0 and 999999.9.");

            RuleFor(x => x.Engine_Power)
                .GreaterThanOrEqualTo(0).WithMessage("Engine power must be a non-negative number.");

            RuleFor(x => x.Carry_Capacity)
                .InclusiveBetween(0, 999999.9m).WithMessage("Carry capacity must be between 0 and 999999.9.");

        }
    }
}
