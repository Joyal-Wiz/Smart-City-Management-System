using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Queries.GetMyIssues
{
    public class GetMyIssuesQuery : IRequest<PagedResult<WorkerIssueDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Status { get; set; } 
        public string? Search { get; set; } 

    }
}