using MediatR;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueCommand : IRequest<ApiResponse<string>>
    {
        public Guid IssueId { get; set; }
    }
}