using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Commands.ApproveWorker
{
    public class ApproveWorkerHandler
        : IRequestHandler<ApproveWorkerCommand, ApiResponse<ApproveWorkerResponseDto>>
    {
        private readonly IWorkerRepository _workerRepository;

        public ApproveWorkerHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<ApproveWorkerResponseDto>> Handle(
            ApproveWorkerCommand request,
            CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

            if (worker == null)
            {
                return ApiResponse<ApproveWorkerResponseDto>.FailResponse(
                    "Worker not found");
            }

            if (worker.Status == WorkerStatus.Approved)
            {
                return ApiResponse<ApproveWorkerResponseDto>.FailResponse(
                    "The worker has already been approved.");
            }

            worker.Status = WorkerStatus.Approved;
            worker.IsAvailable = true;

            await _workerRepository.UpdateAsync(worker);

            var response = new ApproveWorkerResponseDto
            {
                WorkerId = worker.Id,
                Status = worker.Status.ToString(),
                IsAvailable = worker.IsAvailable
            };

            return ApiResponse<ApproveWorkerResponseDto>.SuccessResponse(
                "Worker approved successfully",
                response);
        }
    }
}