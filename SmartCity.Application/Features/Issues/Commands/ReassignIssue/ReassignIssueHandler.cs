using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;

namespace SmartCity.Application.Features.Issues.Commands.ReassignIssue
{
    public class ReassignIssueHandler : IRequestHandler<ReassignIssueCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ReassignIssueHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            ReassignIssueCommand request,
            CancellationToken cancellationToken)
        {
            var issue = await _context.Issues
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

            if (issue == null)
                throw new Exception("Issue not found");

            if (issue.Status != Domain.Enums.IssueStatus.Rejected)
                throw new InvalidOperationException("Only rejected issues can be reassigned");

            var workerExists = await _context.Workers
                .AnyAsync(w => w.Id == request.WorkerId, cancellationToken);

            if (!workerExists)
                throw new Exception("Worker not found");

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

            return true;
        }
    }
}