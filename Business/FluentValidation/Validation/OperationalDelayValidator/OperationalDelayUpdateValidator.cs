using FluentValidation;
using DTO.OperationalDelay;

namespace OperationalDelayValidator
{
    public class OperationalDelayUpdateValidator : AbstractValidator<OperationalDelayUpdateDTO>
    {
        public OperationalDelayUpdateValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Delay_Reason)
                .MaximumLength(1024).WithMessage("Delay reason cannot exceed 1024 characters.");

            RuleFor(x => x.Delay_Duration)
                .MaximumLength(12).WithMessage("Delay duration cannot exceed 12 characters.");

            RuleFor(x => x.Delay_Time)
                .NotEmpty().WithMessage("Delay time is required.");

            RuleFor(x => x.flight_id)
                .NotEmpty().WithMessage("Flight ID is required.");

        }
    }
}
