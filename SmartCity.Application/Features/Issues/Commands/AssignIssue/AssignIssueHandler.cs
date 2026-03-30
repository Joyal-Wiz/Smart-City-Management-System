using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Commands.AssignIssue
{
    public class AssignIssueHandler : IRequestHandler<AssignIssueCommand, ApiResponse<string>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkerRepository _workerRepository;

        public AssignIssueHandler(
            IIssueRepository issueRepository,
            IWorkerRepository workerRepository)
        {
            _issueRepository = issueRepository;
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<string>> Handle(AssignIssueCommand request, CancellationToken cancellationToken)
        {
            // 1. Get Issue
            var issue = await _issueRepository.GetByIdAsync(request.IssueId);
            if (issue == null)
                return ApiResponse<string>.FailResponse("Issue not found");

            // 2. Get Worker
            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
            if (worker == null)
                return ApiResponse<string>.FailResponse("Worker not found");

            // 3. Check worker status
            if (worker.Status != "Approved")
                return ApiResponse<string>.FailResponse("Worker not approved");

            // 4. Assign
            issue.AssignWorker(worker.Id);

            await _issueRepository.UpdateAsync(issue);

            return ApiResponse<string>.SuccessResponse("Issue assigned successfully", "OK");
        }
    }
}