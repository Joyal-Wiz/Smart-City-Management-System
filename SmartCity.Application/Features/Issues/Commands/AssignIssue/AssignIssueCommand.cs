using MediatR;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueCommand : IRequest<ApiResponse<string>>
    {
        public Guid IssueId { get; set; }
        public Guid WorkerId { get; set; }
    }
}