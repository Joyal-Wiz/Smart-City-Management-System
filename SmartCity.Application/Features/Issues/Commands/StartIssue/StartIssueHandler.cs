using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.StartIssue
{
    public class StartIssueHandler
        : IRequestHandler<StartIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ICurrentUserService _currentUser;

        public StartIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            ICurrentUserService currentUser)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> Handle(
            StartIssueCommand request,
            CancellationToken cancellationToken)
        {
            //  1. Get Issue
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            //  2. Get current worker (from user)
            var worker = await _workerRepository
                .GetByUserIdAsync(_currentUser.UserId);

            if (worker == null)
                return ApiResponse<string>.FailResponse("Worker not found");

            //  3. Ownership validation (CORRECT)
            if (issue.AssignedWorkerId != worker.Id)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            //  4. Domain logic
            try
            {
                issue.StartProgress();
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            // 5. Save
            await _issueRepository.UpdateAsync(issue);

            return ApiResponse<string>.SuccessResponse(
                "Issue started successfully",
                "STARTED"
            );
        }
    }
}