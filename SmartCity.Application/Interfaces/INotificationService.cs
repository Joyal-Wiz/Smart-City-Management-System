using System;

namespace SmartCity.Application.Interfaces
{
    public interface INotificationService
    {
        Task CreateAsync(string title, string message, string type, Guid? relatedId = null, Guid? userId = null);
    }

    public interface INotificationRealtimeService
    {
        Task SendAsync(Guid? userId, string message);
    }
}