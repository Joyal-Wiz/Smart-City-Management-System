using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCity.Application.Features.Notifications.Commands.MarkAdminNotificationsAsRead
{
    public class MarkAdminNotificationsAsReadHandler
       : IRequestHandler<MarkAdminNotificationsAsReadCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public MarkAdminNotificationsAsReadHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(
            MarkAdminNotificationsAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == null && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var n in notifications)
            {
                n.IsRead = true;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
