using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadHandler
        : IRequestHandler<MarkAllAsReadCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public MarkAllAsReadHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            MarkAllAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var n in notifications)
                n.IsRead = true;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}