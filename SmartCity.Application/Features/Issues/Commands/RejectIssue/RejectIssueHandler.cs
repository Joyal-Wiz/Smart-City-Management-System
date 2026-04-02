using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.RejectIssue
{
    public class RejectIssueHandler : IRequestHandler<RejectIssueCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public RejectIssueHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            RejectIssueCommand request,
            CancellationToken cancellationToken)
        {
            // 🔍 Get Issue with assignments
            var issue = await _context.Issues
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

            if (issue == null)
                throw new Exception("Issue not found");

            // 🔍 Get Worker from logged-in user
            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.UserId == _currentUser.UserId, cancellationToken);

            if (worker == null)
                throw new UnauthorizedAccessException("Worker not found");

            // 🔒 Validate assignment (CRITICAL SECURITY)
            var isAssigned = issue.Assignments
                .Any(a => a.WorkerId == worker.Id);

            if (!isAssigned)
                throw new UnauthorizedAccessException("You are not assigned to this issue");

            // ⚠️ Optional: Prevent rejecting already completed issues
            if (issue.Status.ToString() == "Resolved")
                throw new InvalidOperationException("Cannot reject a resolved issue");

            // 🔥 Domain logic
            issue.MarkAsRejected(request.Reason);
            // 💾 Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return true; // ✅ YOU MISSED THIS
        }
    }
}