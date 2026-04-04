using FluentValidation;

namespace SmartCity.Application.Features.Issues.Commands.ReassignIssue
{
    public class ReassignIssueCommandValidator : AbstractValidator<ReassignIssueCommand>
    {
        public ReassignIssueCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("IssueId is required");

            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("WorkerId is required");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Deadline must be in the future");

            RuleFor(x => x.Salary)
                .GreaterThan(0)
                .WithMessage("Salary must be greater than 0");
        }
    }
}