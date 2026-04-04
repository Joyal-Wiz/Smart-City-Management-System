using FluentValidation;

namespace SmartCity.Application.Features.Issues.Commands.StartIssue
{
    public class StartIssueCommandValidator : AbstractValidator<StartIssueCommand>
    {
        public StartIssueCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("IssueId is required");
        }
    }
}