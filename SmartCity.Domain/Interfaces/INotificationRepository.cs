using SmartCity.Domain.Entities;

namespace SmartCity.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllAsync();

        Task<int> GetUnreadCountAsync(Guid userId);

        Task MarkAllAsReadAsync(Guid userId);

        Task MarkAsReadAsync(Guid notificationId);

        Task UpdateRangeAsync(List<Notification> notifications);
    }
}