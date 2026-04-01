namespace SmartCity.Application.Features.Workers.DTOs
{
    public class ApproveWorkerResponseDto
    {
        public Guid WorkerId { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}