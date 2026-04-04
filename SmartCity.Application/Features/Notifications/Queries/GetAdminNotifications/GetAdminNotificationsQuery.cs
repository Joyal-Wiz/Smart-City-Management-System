using MediatR;
using SmartCity.Application.Features.Notifications.DTOs;

namespace SmartCity.Application.Features.Notifications.Queries.GetAdminNotifications
{
    public class GetAdminNotificationsQuery : IRequest<List<NotificationDto>>
    {
    }
}