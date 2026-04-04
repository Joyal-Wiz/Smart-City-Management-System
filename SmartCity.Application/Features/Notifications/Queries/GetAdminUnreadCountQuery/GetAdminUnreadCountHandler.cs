using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Notifications.Queries.GetAdminUnreadCount
{
    public class GetAdminUnreadCountHandler
        : IRequestHandler<GetAdminUnreadCountQuery, int>
    {
        private readonly IApplicationDbContext _context;

        public GetAdminUnreadCountHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(
            GetAdminUnreadCountQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(n => n.UserId == null && !n.IsRead)
                .CountAsync(cancellationToken);
        }
    }
}