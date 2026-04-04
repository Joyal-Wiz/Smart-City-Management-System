using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountHandler
        : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetUnreadCountHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            GetUnreadCountQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync(cancellationToken);

            return count;
        }
    }
}