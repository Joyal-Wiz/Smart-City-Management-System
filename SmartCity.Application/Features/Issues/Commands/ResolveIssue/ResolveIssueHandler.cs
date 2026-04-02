using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueHandler
        : IRequestHandler<ResolveIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileService _fileService;

        public ResolveIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            ICurrentUserService currentUser,
            IFileService fileService)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _currentUser = currentUser;
            _fileService = fileService;
        }

        public async Task<ApiResponse<string>> Handle(
            ResolveIssueCommand request,
            CancellationToken cancellationToken)
        {
            // Get Issue
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            // Get Worker from current user
            var worker = await _workerRepository
                .GetByUserIdAsync(_currentUser.UserId);

            if (worker == null)
                return ApiResponse<string>.FailResponse("Worker not found");

            // Ownership validation
            if (issue.AssignedWorkerId != worker.Id)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            //  Validate image
            if (request.Image == null)
                return ApiResponse<string>.FailResponse("Resolved image is required");

            // Save image
            string imagePath;
            try
            {
                imagePath = await _fileService.SaveFileAsync(request.Image);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            // Domain logic
            try
            {
                issue.MarkResolved(imagePath);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            // Save changes
            await _issueRepository.UpdateAsync(issue);

            return ApiResponse<string>.SuccessResponse(
                "Issue resolved with image successfully",
                "RESOLVED"
            );
        }
    }
}