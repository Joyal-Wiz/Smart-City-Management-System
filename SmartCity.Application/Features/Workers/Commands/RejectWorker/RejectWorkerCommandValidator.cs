using FluentValidation;

namespace SmartCity.Application.Features.Workers.Commands.RejectWorker
{
    public class RejectWorkerCommandValidator : AbstractValidator<RejectWorkerCommand>
    {
        public RejectWorkerCommandValidator()
        {
            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("WorkerId is required");
        }
    }
}