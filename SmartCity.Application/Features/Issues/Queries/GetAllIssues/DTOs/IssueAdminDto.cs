using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Queries.GetAllIssues.DTOs
{
    public class IssueAdminDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public IssueType Type { get; set; }
        public IssueStatus Status { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? BeforeImagePath { get; set; }
        public string? AfterImagePath { get; set; }

        public string? AssignedWorkerName { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? Deadline { get; set; }
        public string? RejectionReason { get; set; }
    }
}