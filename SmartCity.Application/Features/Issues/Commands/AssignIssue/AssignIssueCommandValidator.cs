using FluentValidation;
using System;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueCommandValidator : AbstractValidator<AssignIssueCommand>
    {
        public AssignIssueCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("IssueId is required");

            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("WorkerId is required");

            RuleFor(x => x.Deadline)
                .Must(BeFutureDate)
                .WithMessage("Deadline must be in the future");

            RuleFor(x => x.Salary)
                .GreaterThan(0)
                .WithMessage("Salary must be greater than zero");
        }

        private bool BeFutureDate(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}