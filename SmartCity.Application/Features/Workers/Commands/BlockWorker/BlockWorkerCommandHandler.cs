using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCity.Application.Features.Workers.Commands.BlockWorker
{
    public class BlockWorkerCommandHandler : IRequestHandler<BlockWorkerCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public BlockWorkerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(BlockWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _context.Workers
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken);

            if (worker == null) return false;

            worker.Status = WorkerStatus.Blocked;
            worker.BlockReason = request.Reason;
            worker.IsAvailable = false;

            // Send Notification
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = worker.UserId,
                Title = "Account Suspended",
                Message = $"Your operative account has been blocked by administration. Reason: {request.Reason}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
