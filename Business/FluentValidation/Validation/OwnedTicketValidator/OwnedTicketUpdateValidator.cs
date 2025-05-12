using FluentValidation;
using DTO.OwnedTicket;

namespace TicketValidator
{
    public class OwnedTicketUpdateValidator : AbstractValidator<OwnedTicketUpdateDTO>
    {
        public OwnedTicketUpdateValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("ID must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.")
                .LessThanOrEqualTo(99999999.99m).WithMessage("Price cannot exceed 99999999.99.");

            RuleFor(x => x.Baggage_weight)
                .GreaterThanOrEqualTo(0).WithMessage("Baggage weight must be a positive value.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Baggage weight cannot exceed 999999.99.");

            RuleFor(x => x.seat_id)
                .GreaterThan(0).WithMessage("Seat ID must be greater than 0.");

            RuleFor(x => x.Puchase_date)
                .NotEmpty().WithMessage("Purchase date is required.")
                .LessThanOrEqualTo(DateTimeOffset.Now).WithMessage("Purchase date cannot be in the future.");

        }
    }
}
