using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Commands.ReassignIssue
{
    public class ReassignIssueHandler : IRequestHandler<ReassignIssueCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ReassignIssueHandler> _logger;

        public ReassignIssueHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser,
            ILogger<ReassignIssueHandler> logger)
        {
            _context = context;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<bool> Handle(
            ReassignIssueCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("ReassignIssue started for IssueId: {IssueId}, WorkerId: {WorkerId}",
                request.IssueId, request.WorkerId);

            var issue = await _context.Issues
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

            if (issue == null)
            {
                _logger.LogWarning("Reassign failed - Issue not found: {IssueId}", request.IssueId);
                throw new Exception("Issue not found");
            }

            if (issue.Status != IssueStatus.Rejected)
            {
                _logger.LogWarning("Reassign failed - Issue not rejected: {IssueId}", request.IssueId);
                throw new InvalidOperationException("Only rejected issues can be reassigned");
            }

            var workerExists = await _context.Workers
                .AnyAsync(w => w.Id == request.WorkerId, cancellationToken);

            if (!workerExists)
            {
                _logger.LogWarning("Reassign failed - Worker not found: {WorkerId}", request.WorkerId);
                throw new Exception("Worker not found");
            }

            var assignment = IssueAssignment.Create(
                issue.Id,
                request.WorkerId,
                request.Deadline,
                request.Salary,
                _currentUser.UserId
            );

            _context.IssueAssignments.Add(assignment);

            issue.ReassignWorker(request.WorkerId);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Issue {IssueId} reassigned to Worker {WorkerId}",
                request.IssueId, request.WorkerId);

            return true;
        }
    }
}