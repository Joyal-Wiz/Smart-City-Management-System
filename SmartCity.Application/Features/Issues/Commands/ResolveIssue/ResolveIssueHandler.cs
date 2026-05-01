using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using SmartCity.API.Hubs;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueHandler
        : IRequestHandler<ResolveIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<IssueHub> _hubContext;

        public ResolveIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            ICurrentUserService currentUser,
            ICloudinaryService cloudinaryService,
            INotificationService notificationService,
            IHubContext<IssueHub> hubContext // 🔥 FIXED
        )
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _currentUser = currentUser;
            _cloudinaryService = cloudinaryService;
            _notificationService = notificationService;
            _hubContext = hubContext; // 🔥 FIXED
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
                // 🔥 Upload image
                imageUrl = await _cloudinaryService.UploadImageAsync(request.Image);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse($"Image upload failed: {ex.Message}");
            }

            try
            {
                // 🔥 Update issue
                issue.MarkResolved(imageUrl);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }

            // 💾 Save
            await _issueRepository.UpdateAsync(issue);

            // 🔥 SIGNALR EVENT (MOST IMPORTANT)
            await _hubContext.Clients.All.SendAsync("IssueUpdated", new
            {
                issueId = issue.Id,
                status = issue.Status.ToString()
            });

            // 🔔 Notify citizen
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