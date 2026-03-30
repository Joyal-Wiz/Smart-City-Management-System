using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueHandler : IRequestHandler<ResolveIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ICurrentUserService _currentUser;

        public ResolveIssueHandler(
            IIssueRepository issueRepository,
            ICurrentUserService currentUser)
        {
            _issueRepository = issueRepository;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> Handle(ResolveIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            // ✅ NULL check first
            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            // ✅ Ownership check
            if (issue.AssignedWorkerId != _currentUser.UserId)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            // ✅ Domain logic
            issue.MarkResolved();

            await _issueRepository.UpdateAsync(issue);

            return ApiResponse<string>.SuccessResponse("Issue resolved", "OK");
        }
    }
}