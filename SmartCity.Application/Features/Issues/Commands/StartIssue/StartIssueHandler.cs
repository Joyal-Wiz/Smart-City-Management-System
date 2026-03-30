using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.StartIssue
{
    public class StartIssueHandler : IRequestHandler<StartIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ICurrentUserService _currentUser;

        public StartIssueHandler(
            IIssueRepository issueRepository,
            ICurrentUserService currentUser)
        {
            _issueRepository = issueRepository;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> Handle(StartIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            // ✅ FIRST check null
            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            // ✅ THEN check ownership
            if (issue.AssignedWorkerId != _currentUser.UserId)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            // ✅ Domain logic
            issue.StartProgress();

            await _issueRepository.UpdateAsync(issue);

            return ApiResponse<string>.SuccessResponse("Issue started", "OK");
        }
    }
}