using MediatR;
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

        public AssignIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository,
            IIssueAssignmentRepository assignmentRepository,
            ICurrentUserService currentUser)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
            _assignmentRepository = assignmentRepository;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<AssignIssueResponseDto>> Handle(
            AssignIssueCommand request,
            CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);
            if (issue == null)
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Issue not found");

            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
            if (worker == null)
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Worker not found");

            if (worker.Status != WorkerStatus.Approved)
                return ApiResponse<AssignIssueResponseDto>.FailResponse("Worker is not approved");

            try
            {
                issue.AssignWorker(worker.Id);
            }
            catch (Exception ex)
            {
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
                return ApiResponse<AssignIssueResponseDto>.FailResponse(ex.Message);
            }

            await _issueRepository.UpdateAsync(issue);
            await _assignmentRepository.AddAsync(assignment);

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