namespace SmartCity.Application.Features.Issues.DTOs
{
    public class IssueDetailsDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }

        public DateTime? Deadline { get; set; }
        public string AssignedWorkerName { get; set; }
        public bool IsStarted { get; set; }

        public string ResolutionImageUrl { get; set; }
        public DateTime? ResolvedAt { get; set; }

        public string RejectionReason { get; set; }
    }
}