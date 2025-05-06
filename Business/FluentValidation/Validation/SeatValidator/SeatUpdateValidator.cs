using FluentValidation;
using DTO.Seat;

namespace SeatValidator
{
    public class SeatUpdateValidator : AbstractValidator<SeatUpdateDTO>
    {
        public SeatUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Seat_number)
                .GreaterThan(0).WithMessage("Seat number must be greater than 0.");

            RuleFor(x => x.Seat_Class)
                .MaximumLength(32).WithMessage("Seat class cannot exceed 32 characters.");

            RuleFor(x => x.Location)
                .MaximumLength(64).WithMessage("Location cannot exceed 64 characters.");

            RuleFor(x => x.flight_id)
                .NotEmpty().WithMessage("Flight ID is required.");

        }
    }
}
