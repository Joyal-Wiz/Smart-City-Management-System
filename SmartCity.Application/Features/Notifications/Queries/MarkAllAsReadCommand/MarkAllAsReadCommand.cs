using MediatR;

namespace SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommand : IRequest<bool>
    {
    }
}