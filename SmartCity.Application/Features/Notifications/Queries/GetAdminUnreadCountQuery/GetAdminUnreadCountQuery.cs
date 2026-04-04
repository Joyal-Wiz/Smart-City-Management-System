using MediatR;

namespace SmartCity.Application.Features.Notifications.Queries.GetAdminUnreadCount
{
    public class GetAdminUnreadCountQuery : IRequest<int>
    {
    }
}