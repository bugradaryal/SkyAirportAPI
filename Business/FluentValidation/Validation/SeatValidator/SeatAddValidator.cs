using FluentValidation;
using DTO.Seat;

namespace SeatValidator
{
    public class SeatAddValidator : AbstractValidator<SeatAddDTO>
    {
        public SeatAddValidator()
        {
            RuleFor(x => x.Seat_number)
                .NotEmpty().WithMessage("Seat number is required.");

            RuleFor(x => x.Seat_Class)
                .NotEmpty().WithMessage("Seat class is required.")
                .MaximumLength(32).WithMessage("Seat class cannot exceed 32 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(64).WithMessage("Location cannot exceed 64 characters.");

            RuleFor(x => x.flight_id)
                .NotEmpty().WithMessage("Flight ID is required.");

        }
    }
}
