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
        private readonly INotificationService _notificationService;

        public ReassignIssueHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser,
            ILogger<ReassignIssueHandler> logger,
            INotificationService notificationService)
        {
            _context = context;
            _currentUser = currentUser;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(
            ReassignIssueCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "ReassignIssue started for IssueId: {IssueId}, WorkerId: {WorkerId}",
                request.IssueId, request.WorkerId);

            var issue = await _context.Issues
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

            if (issue == null)
            {
                _logger.LogWarning("Reassign failed - Issue not found: {IssueId}", request.IssueId);
                throw new Exception("Issue not found");
            }

            if (issue.Status == IssueStatus.Resolved)
            {
                _logger.LogWarning("Reassign failed - Issue is resolved: {IssueId}", request.IssueId);
                throw new InvalidOperationException("Resolved issues cannot be reassigned");
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken);

            if (worker == null)
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

            try
            {
                await _notificationService.CreateAsync(
                    "Issue Reassigned",
                    $"You have been assigned a reassigned issue. Deadline: {request.Deadline}",
                    "Issue",
                    request.IssueId,
                    worker.UserId 
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("Notification failed: {Message}", ex.Message);
            }

            _logger.LogInformation(
                "Issue {IssueId} reassigned to Worker {WorkerId}",
                request.IssueId, request.WorkerId);

            return true;
        }
    }
}