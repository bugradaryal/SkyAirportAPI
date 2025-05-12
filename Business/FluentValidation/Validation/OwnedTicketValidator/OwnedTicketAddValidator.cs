using FluentValidation;
using DTO.OwnedTicket;

namespace TicketValidator
{
    public class OwnedTicketAddValidator : AbstractValidator<OwnedTicketAddDTO>
    {
        public OwnedTicketAddValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.")
                .LessThanOrEqualTo(99999999.99m).WithMessage("Price cannot exceed 99999999.99.");

            RuleFor(x => x.Baggage_weight)
                .GreaterThanOrEqualTo(0).WithMessage("Baggage weight must be a positive value.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Baggage weight cannot exceed 999999.99.");

            RuleFor(x => x.seat_id)
                .NotEmpty().WithMessage("Seat ID is required.");

        }
    }
}
