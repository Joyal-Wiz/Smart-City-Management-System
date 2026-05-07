using System;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Workers.DTO
{
    public class GetWorkerByIdResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public WorkerStatus Status { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalIssuesAssigned { get; set; }
    }
}
