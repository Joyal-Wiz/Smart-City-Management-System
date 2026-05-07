using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCity.Application.Features.Workers.Commands.UnblockWorker
{
    public class UnblockWorkerCommandHandler : IRequestHandler<UnblockWorkerCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UnblockWorkerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UnblockWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _context.Workers
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken);

            if (worker == null) return false;

            worker.Status = WorkerStatus.Approved;
            worker.BlockReason = null;
            worker.IsAvailable = true;

            // Send Notification
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = worker.UserId,
                Title = "Account Reinstated",
                Message = "Your operative account has been unblocked. You can now resume duties.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
