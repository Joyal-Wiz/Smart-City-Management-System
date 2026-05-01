using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Workers.Queries.GetMyIssues
{
    public class GetMyIssuesQueryHandler
        : IRequestHandler<GetMyIssuesQuery, PagedResult<WorkerIssueDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyIssuesQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<WorkerIssueDto>> Handle(
            GetMyIssuesQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // 🔹 Get worker id
            var workerId = await _context.Workers
                .Where(w => w.UserId == userId)
                .Select(w => w.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (workerId == Guid.Empty)
            {
                throw new Exception("Worker not found");
            }

            // 🔹 Base query
            var query =
                from i in _context.Issues
                join a in _context.IssueAssignments
                    on i.Id equals a.IssueId
                where a.WorkerId == workerId
                select new WorkerIssueDto
                {
                    IssueId = i.Id,
                    Description = i.Description,
                    Location = i.Location.Latitude + ", " + i.Location.Longitude,
                    Status = i.Status,
                    Deadline = a.Deadline,
                    Salary = a.Salary,
                    ImageUrl = i.ImagePath,
                    RejectionReason = i.Status == IssueStatus.Rejected
                        ? i.RejectionReason
                        : null
                };

            // 🔥 STATUS FILTER (NEW)
            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<IssueStatus>(request.Status, true, out var statusEnum))
            {
                query = query.Where(i => i.Status == statusEnum);
            }

            // 🔥 SEARCH FILTER (NEW)
            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                query = query.Where(i => i.Description.ToLower().Contains(searchTerm));
            }

            // 🔹 Total count AFTER filter
            var totalCount = await query.CountAsync(cancellationToken);

            // 🔹 Pagination
            var items = await query
                .OrderByDescending(i => i.IssueId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<WorkerIssueDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}