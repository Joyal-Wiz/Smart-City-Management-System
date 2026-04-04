using MediatR;
using SmartCity.Application.Features.Notifications.DTOs;

namespace SmartCity.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQuery : IRequest<List<NotificationDto>>
    {
    }
}