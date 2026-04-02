using MediatR;

namespace SmartCity.Application.Features.Issues.Commands.RejectIssue
{
    public class RejectIssueCommand : IRequest<bool>
    {
        public Guid IssueId { get; set; }
        public string Reason { get; set; } 

    }
}