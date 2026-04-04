using MediatR;

namespace SmartCity.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountQuery : IRequest<int>
    {
    }
}