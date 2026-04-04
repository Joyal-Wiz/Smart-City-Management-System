using MediatR;
using Microsoft.Extensions.Logging;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueHandler : IRequestHandler<AssignIssueCommand, ApiResponse<AssignIssueResponseDto>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IIssueAssignmentRepository _assignmentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AssignIssueHandler> _logger;

        public AssignIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            IIssueAssignmentRepository assignmentRepository,
            ICurrentUserService currentUser,
            ILogger<AssignIssueHandler> logger)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _assignmentRepository = assignmentRepository;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<ApiResponse<AssignIssueResponseDto>> Handle(
            AssignIssueCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("AssignIssue started for IssueId: {IssueId}, WorkerId: {WorkerId}",
                request.IssueId, request.WorkerId);

            var issue = await _issueRepository.GetByIdAsync(request.IssueId);
            if (issue == null)
            {
                _logger.LogWarning("AssignIssue failed - Issue not found: {IssueId}", request.IssueId);
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Issue not found");
            }

            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
            if (worker == null)
            {
                _logger.LogWarning("AssignIssue failed - Worker not found: {WorkerId}", request.WorkerId);
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Worker not found");
            }

            if (worker.Status != WorkerStatus.Approved)
            {
                _logger.LogWarning("AssignIssue failed - Worker not approved: {WorkerId}", request.WorkerId);
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Worker is not approved");
            }

            try
            {
                issue.AssignWorker(worker.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("AssignIssue domain error: {Message}", ex.Message);
                return ApiResponse<AssignIssueResponseDto>.FailResponse(ex.Message);
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
                _logger.LogWarning("AssignIssue creation failed: {Message}", ex.Message);
                return ApiResponse<AssignIssueResponseDto>.FailResponse(ex.Message);
            }

            await _issueRepository.UpdateAsync(issue);
            await _assignmentRepository.AddAsync(assignment);

            _logger.LogInformation("Issue {IssueId} successfully assigned to Worker {WorkerId}",
                request.IssueId, request.WorkerId);

            var response = new AssignIssueResponseDto
            {
                AssignmentId = assignment.Id,
                IssueId = request.IssueId,
                WorkerId = request.WorkerId,
                Deadline = request.Deadline,
                Salary = request.Salary
            };

            return ApiResponse<AssignIssueResponseDto>.SuccessResponse(
                "Issue assigned successfully",
                response
            );
        }
    }
}