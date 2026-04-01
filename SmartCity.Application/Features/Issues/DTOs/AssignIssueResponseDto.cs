using System;

namespace SmartCity.Application.Features.Issues.DTOs
{
    public class AssignIssueResponseDto
    {
        public Guid AssignmentId { get; set; }

        public Guid IssueId { get; set; }

        public Guid WorkerId { get; set; }

        public DateTime Deadline { get; set; }

        public decimal Salary { get; set; }
    }
}