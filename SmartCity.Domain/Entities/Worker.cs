using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmartCity.Domain.Enums;

namespace SmartCity.Domain.Entities
{
    public class Worker
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public bool IsAvailable { get; set; } = true;

        public WorkerStatus Status { get; set; } = WorkerStatus.Pending;
        [NotMapped]
        public string? BlockReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<IssueAssignment> Assignments { get; set; } = new List<IssueAssignment>();
    }
}