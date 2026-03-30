using MediatR;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.StartIssue
{
    public class StartIssueCommand : IRequest<ApiResponse<string>>
    {
        public Guid IssueId { get; set; }
    }
}