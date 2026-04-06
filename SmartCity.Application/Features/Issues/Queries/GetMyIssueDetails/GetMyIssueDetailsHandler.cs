using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Exceptions;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Queries.GetMyIssueDetails
{
    public class GetMyIssueDetailsHandler
        : IRequestHandler<GetMyIssueDetailsQuery, IssueDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyIssueDetailsHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<IssueDetailsDto> Handle(
            GetMyIssueDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var issue = await _context.Issues
                .Where(i => i.Id == request.IssueId &&
                            i.CreatedByUserId == userId)
                .Select(i => new IssueDetailsDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    ImageUrl = i.ImagePath,
                    Status = i.Status.ToString(),

                    Deadline = i.Assignments
                        .Select(a => a.Deadline)
                        .FirstOrDefault(),

                    AssignedWorkerName = i.Assignments
                        .Select(a => a.Worker.User.Name)
                        .FirstOrDefault(),

                    IsStarted = i.Status == IssueStatus.InProgress,

                    ResolutionImageUrl = i.ResolvedImagePath,
                    ResolvedAt = i.ResolvedAt,

                    RejectionReason = i.RejectionReason
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (issue == null)
                throw new NotFoundException("Issue not found");

            return issue;
        }
    }
}