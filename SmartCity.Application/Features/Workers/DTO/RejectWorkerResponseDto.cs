


namespace SmartCity.Application.Features.Workers.DTOs
{
    public class RejectWorkerResponseDto
    {
        public Guid WorkerId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}