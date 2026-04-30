using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Issues.Queries.GetMyIssuesList
{
    public class GetMyIssuesListHandler
        : IRequestHandler<GetMyIssuesListQuery, PagedResult<IssueListDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyIssuesListHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<IssueListDto>> Handle(
            GetMyIssuesListQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // 🔐 Only user's issues
            var query = _context.Issues
                .Where(i => i.CreatedByUserId == userId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(i => new IssueListDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    Status = i.Status.ToString(),
                    CreatedAt = i.CreatedAt,
                    AssignedWorkerName = i.Assignments
                    .Select(a => a.Worker.User.Name)
                    .FirstOrDefault() ?? "Not Assigned",

                    Latitude = i.Location.Latitude,
                    Longitude = i.Location.Longitude
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<IssueListDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}