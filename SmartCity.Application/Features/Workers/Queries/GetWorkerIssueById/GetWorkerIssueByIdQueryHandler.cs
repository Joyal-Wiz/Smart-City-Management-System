using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Application.Features.Workers.Queries.GetWorkerIssueById;
using SmartCity.Application.Interfaces;

public class GetWorkerIssueByIdQueryHandler
    : IRequestHandler<GetWorkerIssueByIdQuery, WorkerIssueDto>
{
    private readonly IApplicationDbContext _context;

    public GetWorkerIssueByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkerIssueDto> Handle(
        GetWorkerIssueByIdQuery request,
        CancellationToken cancellationToken)
    {
        var issue = await (
            from i in _context.Issues
            join a in _context.IssueAssignments
                on i.Id equals a.IssueId
            where i.Id == request.Id
            select new WorkerIssueDto
            {
                IssueId = i.Id,
                Description = i.Description,
                Location = i.Location.Latitude + ", " + i.Location.Longitude,
                Status = i.Status,
                Deadline = a.Deadline,
                Salary = a.Salary,

                // 🔥 BEFORE IMAGE
                ImageUrl = i.ImagePath,

                // 🔥 AFTER IMAGE (CORRECT FIELD)
                ResolutionImageUrl = i.ResolvedImagePath,

                RejectionReason = i.RejectionReason
            }
        ).FirstOrDefaultAsync(cancellationToken);

        if (issue == null)
            throw new Exception("Issue not found");

        return issue;
    }
}