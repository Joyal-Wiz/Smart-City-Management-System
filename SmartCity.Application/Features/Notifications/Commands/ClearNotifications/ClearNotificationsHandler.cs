using MediatR;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Notifications.Commands.ClearNotifications
{
    public class ClearNotificationsHandler
        : IRequestHandler<ClearNotificationsCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public ClearNotificationsHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(
            ClearNotificationsCommand request,
            CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetAllAsync();

            if (notifications == null || !notifications.Any())
                return true;

            foreach (var n in notifications)
            {
                n.IsDeleted = true;
            }

            await _notificationRepository.UpdateRangeAsync(notifications);

            return true;
        }
    }
}