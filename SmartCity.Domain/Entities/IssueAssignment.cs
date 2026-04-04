using System;

namespace SmartCity.Domain.Entities
{
    public class IssueAssignment
    {
        public Guid Id { get; private set; }

        public Issue Issue { get; set; }

        public Guid IssueId { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; } 

        public DateTime Deadline { get; private set; }

        public decimal Salary { get; private set; }

        public DateTime AssignedAt { get; private set; }

        public Guid AssignedBy { get; private set; }
        public bool IsDeadlineNotified { get; set; } = false;

        private IssueAssignment() { }

        public static IssueAssignment Create(
            Guid issueId,
            Guid workerId,
            DateTime deadline,
            decimal salary,
            Guid assignedBy)
        {
            if (issueId == Guid.Empty)
                throw new ArgumentException("IssueId is required");

            if (workerId == Guid.Empty)
                throw new ArgumentException("WorkerId is required");

            if (assignedBy == Guid.Empty)
                throw new ArgumentException("AssignedBy is required");

            if (deadline <= DateTime.UtcNow)
                throw new ArgumentException("Deadline must be in the future");

            if (salary <= 0)
                throw new ArgumentException("Salary must be greater than zero");

            return new IssueAssignment
            {
                Id = Guid.NewGuid(),
                IssueId = issueId,
                WorkerId = workerId,
                Deadline = deadline,
                Salary = salary,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = assignedBy
            };
        }
    }
}