using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.DTOs;

namespace SmartCity.Application.Features.Issues.Queries.GetMyIssuesList
{
    public record GetMyIssuesListQuery(int PageNumber, int PageSize)
        : IRequest<PagedResult<IssueListDto>>;
}