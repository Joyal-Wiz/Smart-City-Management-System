using System;

namespace SmartCity.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public string? Type { get; set; } // Issue, Worker, etc.
        public Guid? UserId { get; set; } // 🔥 WHO SHOULD SEE THIS

        public string? UniqueKey { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? RelatedEntityId { get; set; }

    }
}