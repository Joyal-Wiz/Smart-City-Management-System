using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Queries.GetMyIssues
{
    public class GetMyIssuesQuery : IRequest<ApiResponse<PagedResult<WorkerIssueDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}