using FluentValidation;

namespace SmartCity.Application.Features.Issues.Commands.RejectIssue
{
    public class RejectIssueCommandValidator : AbstractValidator<RejectIssueCommand>
    {
        public RejectIssueCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("IssueId is required");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Rejection reason is required")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
        }
    }
}