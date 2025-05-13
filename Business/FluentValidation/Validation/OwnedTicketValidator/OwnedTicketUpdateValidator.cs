using FluentValidation;
using DTO.OwnedTicket;

namespace OwnedTicketValidator
{
    public class TicketUpdateValidator : AbstractValidator<OwnedTicketUpdateDTO>
    {
        public TicketUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Baggage_weight)
                .GreaterThanOrEqualTo(0).WithMessage("Baggage weight must be a positive value.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Baggage weight cannot exceed 999999.99.");
            RuleFor(x => x.user_id)
                 .NotEmpty().WithMessage("UserID is required.");
            RuleFor(x => x.ticket_id)
                 .NotEmpty().WithMessage("TicketID is required.");
        
        }
    }
}
