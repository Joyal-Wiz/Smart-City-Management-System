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
        private readonly ICloudinaryService _cloudinaryService; // 🔥 FIXED
        private readonly INotificationService _notificationService;

        public ResolveIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            ICurrentUserService currentUser,
            ICloudinaryService cloudinaryService, // 🔥 FIXED
            INotificationService notificationService)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _currentUser = currentUser;
            _cloudinaryService = cloudinaryService; // 🔥 FIXED
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<string>> Handle(
            ResolveIssueCommand request,
            CancellationToken cancellationToken)
        {
            // 🔍 Get issue
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);

            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            // 🔍 Get worker
            var worker = await _workerRepository
                .GetByUserIdAsync(_currentUser.UserId);

            if (worker == null)
                return ApiResponse<string>.FailResponse("Worker not found");

            // 🔐 Check ownership
            if (issue.AssignedWorkerId != worker.Id)
                return ApiResponse<string>.FailResponse("You are not assigned to this issue");

            // 🚫 Validate image
            if (request.Image == null)
                return ApiResponse<string>.FailResponse("Resolved image is required");

            string? imageUrl;

            try
            {
                // 🔥 CLOUDINARY UPLOAD
                imageUrl = await _cloudinaryService.UploadImageAsync(request.Image);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse($"Image upload failed: {ex.Message}");
            }

            try
            {
                // 🔥 SAVE CLOUDINARY URL
                issue.MarkResolved(imageUrl);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            await _issueRepository.UpdateAsync(issue);

            // 🔔 Notify Citizen
            await _notificationService.CreateAsync(
                "Issue Resolved",
                "Your reported issue has been resolved.",
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