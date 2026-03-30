using System;
using System.Collections.Generic;
using System.Text;
using SmartCity.Domain.Enums;
using SmartCity.Domain.ValueObjects;

namespace SmartCity.Domain.Entities
{

    public class Issue
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; private set; } = IssueStatus.Reported;

        public Location Location { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid? AssignedWorkerId { get; private set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; private set; }
        public string? ImagePath { get; set; }


        public void AssignWorker(Guid workerId)
        {
            if (Status != IssueStatus.Reported)
                throw new Exception("Issue must be in Reported state");

            AssignedWorkerId = workerId;
            Status = IssueStatus.Assigned;
        }

        public void StartProgress()
        {
            if (Status != IssueStatus.Assigned)
                throw new Exception("Issue must be assigned first");

            Status = IssueStatus.InProgress;
        }

        public void MarkResolved()
        {
            if (Status != IssueStatus.InProgress)
                throw new Exception("Issue must be in progress");

            Status = IssueStatus.Resolved;
            ResolvedAt = DateTime.UtcNow;
        }
    }
}
