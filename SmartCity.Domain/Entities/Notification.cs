using System;

namespace SmartCity.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public string? Type { get; set; } 
        public Guid? UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string? UniqueKey { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? RelatedEntityId { get; set; }

    }
}