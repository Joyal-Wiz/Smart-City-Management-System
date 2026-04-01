using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Commands.RejectWorker
{
    public class RejectWorkerHandler
        : IRequestHandler<RejectWorkerCommand, ApiResponse<RejectWorkerResponseDto>>
    {
        private readonly IWorkerRepository _workerRepository;

        public RejectWorkerHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<RejectWorkerResponseDto>> Handle(
            RejectWorkerCommand request,
            CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

            if (worker == null)
            {
                return ApiResponse<RejectWorkerResponseDto>.FailResponse(
                    "Worker not found");
            }

            if (worker.Status == WorkerStatus.Rejected)
            {
                return ApiResponse<RejectWorkerResponseDto>.FailResponse(
                    "The worker has already been rejected.");
            }

            if (worker.Status == WorkerStatus.Approved)
            {
                return ApiResponse<RejectWorkerResponseDto>.FailResponse(
                    "Approved workers cannot be rejected.");
            }

            worker.Status = WorkerStatus.Rejected;
            worker.IsAvailable = false;

            await _workerRepository.UpdateAsync(worker);

            var response = new RejectWorkerResponseDto
            {
                WorkerId = worker.Id,
                Status = worker.Status.ToString()
            };

            return ApiResponse<RejectWorkerResponseDto>.SuccessResponse(
                "Worker rejected successfully",
                response);
        }
    }
}