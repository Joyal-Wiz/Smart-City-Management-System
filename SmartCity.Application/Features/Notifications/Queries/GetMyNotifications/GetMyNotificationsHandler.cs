using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Application.Features.Notifications.DTOs;

namespace SmartCity.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsHandler
        : IRequestHandler<GetMyNotificationsQuery, List<NotificationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyNotificationsHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<NotificationDto>> Handle(
            GetMyNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var role = _currentUser.Role; // 🔥 IMPORTANT

            var query = _context.Notifications.AsQueryable();

            // ✅ ROLE-BASED FILTER
            if (role == "Admin")
            {
                query = query.Where(n => n.UserId == null);
            }
            else
            {
                query = query.Where(n => n.UserId == userId);
            }

            var notifications = await query
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return notifications;
        }
    }
}