using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Features.Notifications.DTOs;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Notifications.Queries.GetAdminNotifications
{
    public class GetAdminNotificationsHandler
        : IRequestHandler<GetAdminNotificationsQuery, List<NotificationDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAdminNotificationsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> Handle(
            GetAdminNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(n => n.UserId == null) // 🔥 ADMIN
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
        }
    }
}