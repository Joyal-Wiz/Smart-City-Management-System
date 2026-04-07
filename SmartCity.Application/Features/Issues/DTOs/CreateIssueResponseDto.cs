using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.DTOs
{
    public class CreateIssueResponseDto
    {
        public Guid IssueId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}