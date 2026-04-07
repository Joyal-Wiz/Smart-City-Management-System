using MediatR;
using Microsoft.Extensions.Logging;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;
using SmartCity.Application.Exceptions;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueHandler : IRequestHandler<AssignIssueCommand, AssignIssueResponseDto>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IIssueAssignmentRepository _assignmentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AssignIssueHandler> _logger;
        private readonly INotificationService _notificationService;

        public AssignIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            IIssueAssignmentRepository assignmentRepository,
            ICurrentUserService currentUser,
            ILogger<AssignIssueHandler> logger,
            INotificationService notificationService)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _assignmentRepository = assignmentRepository;
            _currentUser = currentUser;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<AssignIssueResponseDto> Handle(
            AssignIssueCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("AssignIssue started for IssueId: {IssueId}, WorkerId: {WorkerId}",
                request.IssueId, request.WorkerId);

            var issue = await _issueRepository.GetByIdAsync(request.IssueId);
            if (issue == null)
                throw new NotFoundException("Issue not found");

            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
            if (worker == null)
                throw new NotFoundException("Worker not found");

            if (worker.Status != WorkerStatus.Approved)
                throw new BadRequestException("Worker is not approved");

            try
            {
                issue.AssignWorker(worker.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("AssignIssue domain error: {Message}", ex.Message);
                throw new BadRequestException(ex.Message);
            }

            IssueAssignment assignment;
            try
            {
                assignment = IssueAssignment.Create(
                    request.IssueId,
                    request.WorkerId,
                    request.Deadline,
                    request.Salary,
                    _currentUser.UserId
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Assignment creation failed: {Message}", ex.Message);
                throw new BadRequestException(ex.Message);
            }

            await _issueRepository.UpdateAsync(issue);
            await _assignmentRepository.AddAsync(assignment);

            // 🔔 Admin notification
            await _notificationService.CreateAsync(
                "Issue Assigned",
                $"Issue assigned to worker with ID: {request.WorkerId}",
                "Issue",
                request.IssueId
            );

            // 🔔 Worker notification
            await _notificationService.CreateAsync(
                "New Work Assigned",
                $"You have been assigned a new issue. Deadline: {request.Deadline}",
                "Issue",
                request.IssueId,
                worker.UserId
            );

            _logger.LogInformation("Issue {IssueId} assigned to Worker {WorkerId}",
                request.IssueId, request.WorkerId);

            return new AssignIssueResponseDto
            {
                AssignmentId = assignment.Id,
                IssueId = request.IssueId,
                WorkerId = request.WorkerId,
                Deadline = request.Deadline,
                Salary = request.Salary
            };
        }
    }
}