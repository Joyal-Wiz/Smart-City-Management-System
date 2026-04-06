using MediatR;
using SmartCity.Application.Features.Issues.DTOs;

namespace SmartCity.Application.Features.Issues.Queries.GetMyIssueDetails
{
    public record GetMyIssueDetailsQuery(Guid IssueId)
        : IRequest<IssueDetailsDto>;
}