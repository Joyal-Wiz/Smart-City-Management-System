namespace SmartCity.Application.Features.Issues.DTOs
{
    public class IssueListDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AssignedWorkerName { get; set; }

        // 🔥 ADD THESE
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}