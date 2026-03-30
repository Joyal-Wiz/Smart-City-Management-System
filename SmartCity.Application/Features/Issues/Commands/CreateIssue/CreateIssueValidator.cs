using FluentValidation;

namespace SmartCity.Application.Features.Issues.Commands.CreateIssue
{
    public class CreateIssueValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid issue type");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Invalid latitude");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Invalid longitude");
        }
    }
}