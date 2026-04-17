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
        private readonly INotificationService _notificationService;

        public ResolveIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            ICurrentUserService currentUser,
            IFileService fileService,
            INotificationService notificationService)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _currentUser = currentUser;
            _fileService = fileService;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<string>> Handle(
            ResolveIssueCommand request,
            CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            var worker = await _workerRepository
                .GetByUserIdAsync(_currentUser.UserId);

            if (worker == null)
                return ApiResponse<string>.FailResponse("Worker not found");

            if (issue.AssignedWorkerId != worker.Id)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            if (request.Image == null)
                return ApiResponse<string>.FailResponse("Resolved image is required");

            string imagePath;
            try
            {
                imagePath = await _fileService.SaveFileAsync(request.Image);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            try
            {
                issue.MarkResolved(imagePath);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            await _issueRepository.UpdateAsync(issue);

            // 🔔 Notify Citizen
            await _notificationService.CreateAsync(
                "Issue Resolved",
                $"Your reported issue has been resolved.",
                "Issue",
                issue.Id,
                issue.CreatedByUserId
            );

            return ApiResponse<string>.SuccessResponse(
                "Issue resolved successfully",
                "RESOLVED"
            );
        }
    }
}