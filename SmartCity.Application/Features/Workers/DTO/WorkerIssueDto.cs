using System;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Workers.DTOs
{
    public class WorkerIssueDto
    {
        public Guid IssueId { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }
        public DateTime Deadline { get; set; }

        public decimal Salary { get; set; }

        public IssueStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? RejectionReason { get; set; }
    }
}