using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;
using SmartCity.Domain.ValueObjects;

namespace SmartCity.Application.Features.Issues.Commands.CreateIssue
{
    public class CreateIssueHandler : IRequestHandler<CreateIssueCommand, ApiResponse<Guid>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _notificationService;

        public CreateIssueHandler(
            IIssueRepository issueRepository,
            IFileService fileService,
            ICurrentUserService currentUser,
            INotificationService notificationService)
        {
            _issueRepository = issueRepository;
            _fileService = fileService;
            _currentUser = currentUser;
            _notificationService = notificationService; // ✅ FIXED
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateIssueCommand request,
            CancellationToken cancellationToken)
        {
            var location = new Location(request.Latitude, request.Longitude);

            string? imagePath = null;

            if (request.Image is not null)
            {
                imagePath = await _fileService.SaveFileAsync(request.Image);
            }

            var issue = new Issue
            {
                Id = Guid.NewGuid(),
                Description = request.Description.Trim(),
                Type = request.Type,
                Location = location,
                CreatedByUserId = _currentUser.UserId,
                ImagePath = imagePath
            };

            await _issueRepository.AddAsync(issue);

            // 🔥 ADD NOTIFICATION (IMPORTANT)
            await _notificationService.CreateAsync(
                "New Issue Reported",
                $"A new issue has been reported: {issue.Description}",
                "Issue",
                issue.Id
            );

            return ApiResponse<Guid>.SuccessResponse(
                "Issue created successfully",
                issue.Id
            );
        }
    }
}