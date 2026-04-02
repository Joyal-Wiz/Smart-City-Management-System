using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Queries.GetAllIssues.DTOs;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Queries.GetAllIssues
{
    public class GetAllIssuesQuery : IRequest<PagedResult<IssueAdminDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IssueStatus? Status { get; set; }
        public IssueType? Type { get; set; }
        public string? Search { get; set; }
    }
}