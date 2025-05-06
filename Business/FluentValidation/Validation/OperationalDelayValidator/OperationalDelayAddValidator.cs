using FluentValidation;
using DTO.OperationalDelay;

namespace OperationalDelayValidator
{
    public class OperationalDelayAddValidator : AbstractValidator<OperationalDelayAddDTO>
    {
        public OperationalDelayAddValidator()
        {
            RuleFor(x => x.Delay_Reason)
                .NotEmpty().WithMessage("Delay reason is required.")
                .MaximumLength(1024).WithMessage("Delay reason cannot exceed 1024 characters.");

            RuleFor(x => x.Delay_Duration)
                .NotEmpty().WithMessage("Delay duration is required.")
                .MaximumLength(12).WithMessage("Delay duration cannot exceed 12 characters.");

            RuleFor(x => x.Delay_Time)
                .NotEmpty().WithMessage("Delay time is required.");

            RuleFor(x => x.flight_id)
                .NotEmpty().WithMessage("Flight ID is required.");

        }
    }
}
