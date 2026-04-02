using System;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Workers.DTOs
{
    public class WorkerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public WorkerStatus Status { get; set; }

        public bool IsAvailable { get; set; }
    }
}