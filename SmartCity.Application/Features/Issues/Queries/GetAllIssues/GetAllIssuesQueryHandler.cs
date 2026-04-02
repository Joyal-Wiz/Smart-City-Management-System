using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Application.Features.Issues.Queries.GetAllIssues.DTOs;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Queries.GetAllIssues
{
    public class GetAllIssuesQueryHandler
        : IRequestHandler<GetAllIssuesQuery, PagedResult<IssueAdminDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllIssuesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<IssueAdminDto>> Handle(
            GetAllIssuesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Issues
                .Include(i => i.Assignments)
                    .ThenInclude(a => a.Worker)
                        .ThenInclude(w => w.User)
                .AsQueryable();

            // 🔍 FILTER: STATUS
            if (request.Status.HasValue)
            {
                query = query.Where(i => i.Status == request.Status.Value);
            }

            // 🔍 FILTER: TYPE
            if (request.Type.HasValue)
            {
                query = query.Where(i => i.Type == request.Type.Value);
            }

            // 🔍 SEARCH (Optimized)
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(i =>
                    EF.Functions.Like(i.Description, $"%{request.Search}%"));
            }

            // 📊 TOTAL COUNT (after filtering)
            var totalCount = await query.CountAsync(cancellationToken);

            // 📦 DATA FETCH
            var issues = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
               .Select(i => new IssueAdminDto
               {
                   Id = i.Id,
                   Description = i.Description,
                   Type = i.Type,
                   Status = i.Status,

                   Latitude = i.Location.Latitude,
                   Longitude = i.Location.Longitude,

                   BeforeImagePath = i.ImagePath,
                   AfterImagePath = i.ResolvedImagePath,

                   AssignedWorkerName = i.Assignments
        .Select(a => a.Worker.User.Name)
        .FirstOrDefault(),

                   Salary = i.Assignments
        .Select(a => a.Salary)
        .FirstOrDefault(),

                   Deadline = i.Assignments
        .Select(a => a.Deadline)
        .FirstOrDefault(),

                   RejectionReason = i.RejectionReason
               })
                .ToListAsync(cancellationToken);

            return new PagedResult<IssueAdminDto>
            {
                Items = issues,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}