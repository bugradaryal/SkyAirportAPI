using FluentValidation;
using DTO.OwnedTicket;
using DTO.Ticket;

namespace TicketValidator
{
    public class TicketUpdateValidator : AbstractValidator<TicketUpdateDTO>
    {
        public TicketUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.")
                .LessThanOrEqualTo(99999999.99m).WithMessage("Price cannot exceed 99999999.99.");

            RuleFor(x => x.seat_id)
                .GreaterThan(0).WithMessage("Seat ID must be greater than 0.");
        }
    }
}
