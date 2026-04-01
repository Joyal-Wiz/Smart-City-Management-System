using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueCommand : IRequest<ApiResponse<AssignIssueResponseDto>>
    {
        public Guid IssueId { get; set; }
        public Guid WorkerId { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Salary { get; set; }
    }
}