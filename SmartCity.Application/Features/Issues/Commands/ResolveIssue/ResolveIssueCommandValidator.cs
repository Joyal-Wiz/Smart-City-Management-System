using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueCommandValidator : AbstractValidator<ResolveIssueCommand>
    {
        public ResolveIssueCommandValidator()
        {
            RuleFor(x => x.IssueId)
                .NotEmpty().WithMessage("IssueId is required");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Resolved image is required")
                .Must(BeValidFileType).WithMessage("Only JPG and PNG are allowed")
                .Must(BeValidFileSize).WithMessage("File size must be less than 5MB");
        }

        private bool BeValidFileType(IFormFile file)
        {
            if (file == null) return false;

            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return allowedTypes.Contains(file.ContentType);
        }

        private bool BeValidFileSize(IFormFile file)
        {
            if (file == null) return false;

            return file.Length <= 5 * 1024 * 1024; // 5MB
        }
    }
}