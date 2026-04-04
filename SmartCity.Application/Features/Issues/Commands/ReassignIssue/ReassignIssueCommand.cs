using MediatR;

namespace SmartCity.Application.Features.Issues.Commands.ReassignIssue
{
    public class ReassignIssueCommand : IRequest<bool>
    {
        public Guid IssueId { get; set; }
        public Guid WorkerId { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Salary { get; set; }
    }
}