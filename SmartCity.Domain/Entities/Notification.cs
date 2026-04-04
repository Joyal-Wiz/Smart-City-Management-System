using System;

namespace SmartCity.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔥 Optional: Link to related entity
        public Guid? RelatedEntityId { get; set; }

        public string? Type { get; set; } // Issue, Worker, etc.
    }
}