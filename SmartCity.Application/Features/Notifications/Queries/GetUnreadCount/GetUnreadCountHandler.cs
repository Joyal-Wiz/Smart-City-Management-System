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
            var role = _currentUser.Role;

            var query = _context.Notifications.AsQueryable();

            if (role == "Admin")
            {
                query = query.Where(n => n.UserId == null && !n.IsRead);
            }
            else
            {
                query = query.Where(n => n.UserId == userId && !n.IsRead);
            }

            return await query.CountAsync(cancellationToken);
        }
    }
}