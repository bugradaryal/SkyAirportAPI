using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Aircraft;
using FluentValidation;

namespace Business.FluentValidation.Validation.AircraftValidator
{
    public class AircraftAddValidator :AbstractValidator<AircraftAddDto>
    {
        public AircraftAddValidator() 
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(64).WithMessage("Model must be at most 64 characters.");

            RuleFor(x => x.Fuel_Capacity)
                .NotNull().WithMessage("Fuel capacity is required.")
                .InclusiveBetween(0, 999999.9m).WithMessage("Fuel capacity must be between 0 and 999999.9.");

            RuleFor(x => x.Max_Altitude)
                .NotNull().WithMessage("Max altitude is required.")
                .InclusiveBetween(0, 9999999.9m).WithMessage("Max altitude must be between 0 and 9999999.9.");

            RuleFor(x => x.Engine_Power)
                .NotNull().WithMessage("Engine power is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Engine power must be a non-negative number.");

            RuleFor(x => x.Carry_Capacity)
                .NotNull().WithMessage("Carry capacity is required.")
                .InclusiveBetween(0, 99999.99m).WithMessage("Carry capacity must be between 0 and 99999.99.");

            RuleFor(x => x.aircraftStatus_id)
                .NotNull().WithMessage("Aircraft status ID is required.");

        }
    }
}
