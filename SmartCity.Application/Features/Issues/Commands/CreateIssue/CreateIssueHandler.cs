using MediatR;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;
using SmartCity.Domain.ValueObjects;

namespace SmartCity.Application.Features.Issues.Commands.CreateIssue
{
    public class CreateIssueHandler
        : IRequestHandler<CreateIssueCommand, CreateIssueResponseDto>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _notificationService;
        private readonly ICloudinaryService _cloudinaryService; // ✅ FIXED

        public CreateIssueHandler(
            IIssueRepository issueRepository,
            ICurrentUserService currentUser,
            INotificationService notificationService,
            ICloudinaryService cloudinaryService // ✅ FIXED
        )
        {
            _issueRepository = issueRepository;
            _currentUser = currentUser;
            _notificationService = notificationService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<CreateIssueResponseDto> Handle(
            CreateIssueCommand request,
            CancellationToken cancellationToken)
        {
            var location = new Location(request.Latitude, request.Longitude);

            string? imageUrl = null;

            // 🔥 Upload to Cloudinary
            if (request.Image is not null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(request.Image);
            }

            var issue = new Issue
            {
                Id = Guid.NewGuid(),
                Description = request.Description.Trim(),
                Type = request.Type,
                Location = location,
                CreatedByUserId = _currentUser.UserId,

                // 🔥 IMPORTANT: now storing FULL URL
                ImagePath = imageUrl
            };

            await _issueRepository.AddAsync(issue);

            await _notificationService.CreateAsync(
                "New Issue Reported",
                $"A new issue has been reported: {issue.Description}",
                "Issue",
                issue.Id
            );

            return new CreateIssueResponseDto
            {
                IssueId = issue.Id,
                Status = issue.Status.ToString()
            };
        }
    }
}