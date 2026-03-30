namespace SmartCity.Application.DTOs
{
    public class WorkerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}