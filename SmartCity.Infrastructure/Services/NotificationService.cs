using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;

public class NotificationService : INotificationService
{
    private readonly IApplicationDbContext _context;

    public NotificationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(string title, string message, string type, Guid? relatedId = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Title = title,
            Message = message,
            Type = type,
            RelatedEntityId = relatedId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}