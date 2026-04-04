using FluentValidation;

namespace SmartCity.Application.Features.Workers.Commands.ApproveWorker
{
    public class ApproveWorkerCommandValidator : AbstractValidator<ApproveWorkerCommand>
    {
        public ApproveWorkerCommandValidator()
        {
            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("WorkerId is required");
        }
    }
}