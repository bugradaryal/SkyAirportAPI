using FluentValidation;
using DTO.Flight;

namespace FlightValidator
{
    public class FlightAddValidator : AbstractValidator<FlightAddDTO>
    {
        public FlightAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(128).WithMessage("Name cannot exceed 128 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1024).WithMessage("Description cannot exceed 1024 characters.");

            RuleFor(x => x.Arrival_Date)
                .NotEmpty().WithMessage("Arrival date is required.");

            RuleFor(x => x.Deperture_Date)
                .NotEmpty().WithMessage("Departure date is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(38).WithMessage("Status cannot exceed 38 characters.");

            RuleFor(x => x.airline_id)
                .NotEmpty().WithMessage("Airline ID is required.");

            RuleFor(x => x.aircraft_id)
                .NotEmpty().WithMessage("Aircraft ID is required.");

        }
    }
}
