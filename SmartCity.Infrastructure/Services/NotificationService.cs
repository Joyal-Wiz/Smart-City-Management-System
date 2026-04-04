using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;

namespace SmartCity.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IApplicationDbContext _context;

        public NotificationService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(
            string title,
            string message,
            string type,
            Guid? relatedId = null,
            Guid? userId = null)
        {
            var uniqueKey = $"{type}-{relatedId}-{userId}-{title}";

            var exists = _context.Notifications
                .Any(n => n.UniqueKey == uniqueKey);

            if (exists)
                return; // 🔥 PREVENT DUPLICATE

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                Type = type,
                RelatedEntityId = relatedId,
                UserId = userId,
                UniqueKey = uniqueKey, // 🔥 ADD
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}