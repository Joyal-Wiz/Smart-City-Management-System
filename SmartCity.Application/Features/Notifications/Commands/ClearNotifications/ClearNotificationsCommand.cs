using MediatR;

namespace SmartCity.Application.Features.Notifications.Commands.ClearNotifications
{
    public record ClearNotificationsCommand : IRequest<bool>;
}