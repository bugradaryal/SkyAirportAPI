using FluentValidation;
using DTO.OwnedTicket;
using DTO.Ticket;

namespace TicketValidator
{
    public class TicketAddValidator : AbstractValidator<TicketAddDTO>
    {
        public TicketAddValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.")
                .LessThanOrEqualTo(99999999.99m).WithMessage("Price cannot exceed 99999999.99.");
            RuleFor(x => x.seat_id)
                .NotEmpty().WithMessage("Seat ID is required.");

        }
    }
}
