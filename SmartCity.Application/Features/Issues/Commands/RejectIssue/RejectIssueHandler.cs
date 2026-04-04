using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Commands.RejectIssue
{
    public class RejectIssueHandler : IRequestHandler<RejectIssueCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<RejectIssueHandler> _logger;

        public RejectIssueHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser,
            ILogger<RejectIssueHandler> logger)
        {
            _context = context;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<bool> Handle(
            RejectIssueCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("RejectIssue started for IssueId: {IssueId}", request.IssueId);

            var issue = await _context.Issues
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

            if (issue == null)
            {
                _logger.LogWarning("RejectIssue failed - Issue not found: {IssueId}", request.IssueId);
                throw new Exception("Issue not found");
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.UserId == _currentUser.UserId, cancellationToken);

            if (worker == null)
            {
                _logger.LogWarning("RejectIssue failed - Worker not found for UserId: {UserId}", _currentUser.UserId);
                throw new UnauthorizedAccessException("Worker not found");
            }

            var isAssigned = issue.Assignments
                .Any(a => a.WorkerId == worker.Id);

            if (!isAssigned)
            {
                _logger.LogWarning("Unauthorized reject attempt by WorkerId: {WorkerId} on IssueId: {IssueId}",
                    worker.Id, request.IssueId);
                throw new UnauthorizedAccessException("You are not assigned to this issue");
            }

            if (issue.Status == IssueStatus.Resolved)
            {
                _logger.LogWarning("Reject failed - Issue already resolved: {IssueId}", request.IssueId);
                throw new InvalidOperationException("Cannot reject a resolved issue");
            }

            issue.MarkAsRejected(request.Reason);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Issue {IssueId} rejected by WorkerId: {WorkerId} with reason: {Reason}",
                request.IssueId, worker.Id, request.Reason);

            return true;
        }
    }
}