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
            _logger.LogInformation(
                "AssignIssue started for IssueId: {IssueId}, WorkerId: {WorkerId}",
                request.IssueId, request.WorkerId);

            // 🔹 GET ISSUE
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);
            if (issue == null)
                throw new NotFoundException("Issue not found");

            // 🔹 PREVENT RE-ASSIGN
            if (issue.Status == IssueStatus.Assigned)
                throw new BadRequestException("Issue is already assigned");

            // 🔹 GET WORKER
            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
            if (worker == null)
                throw new NotFoundException("Worker not found");

            // 🔹 VALIDATION
            if (worker.Status != WorkerStatus.Approved)
                throw new BadRequestException("Worker is not approved");

            // 🔥 DERIVED AVAILABILITY
            var activeCount = await _assignmentRepository.GetActiveAssignmentsCount(worker.Id);

            if (activeCount >= 4)
                throw new BadRequestException("Worker is busy (max 4 tasks)");

            // 🔹 DOMAIN ASSIGN
            try
            {
                issue.AssignWorker(worker.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("AssignIssue domain error: {Message}", ex.Message);
                throw new BadRequestException(ex.Message);
            }

            // 🔹 CREATE ASSIGNMENT
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

            // 🔹 SAVE CORE DATA (CRITICAL PART)
            await _issueRepository.UpdateAsync(issue);
            await _assignmentRepository.AddAsync(assignment);

            // 🔔 NOTIFICATIONS (NON-CRITICAL → SAFE WRAP)
            try
            {
                // Admin notification
                await _notificationService.CreateAsync(
                    "Issue Assigned",
                    $"Issue assigned to worker with ID: {request.WorkerId}",
                    "Issue",
                    request.IssueId
                );

                // Worker notification
                await _notificationService.CreateAsync(
                    "New Work Assigned",
                    $"You have been assigned a new issue. Deadline: {request.Deadline}",
                    "Issue",
                    request.IssueId,
                    worker.UserId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("Notification failed: {Message}", ex.Message);
                // ❗ Do NOT throw — assignment already succeeded
            }

            _logger.LogInformation(
                "Issue {IssueId} successfully assigned to Worker {WorkerId}",
                request.IssueId, request.WorkerId);

            // 🔹 RESPONSE
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