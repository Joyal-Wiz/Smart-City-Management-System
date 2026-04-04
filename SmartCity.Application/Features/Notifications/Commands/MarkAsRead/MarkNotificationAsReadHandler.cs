using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkNotificationAsReadHandler
        : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public MarkNotificationAsReadHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == request.NotificationId, cancellationToken);

            if (notification == null)
                throw new Exception("Notification not found");

            notification.IsRead = true;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}