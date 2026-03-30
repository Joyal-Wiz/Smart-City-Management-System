using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Commands.ApproveWorker
{
    public class ApproveWorkerHandler : IRequestHandler<ApproveWorkerCommand, ApiResponse<string>>
    {
        private readonly IWorkerRepository _workerRepository;

        public ApproveWorkerHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<string>> Handle(ApproveWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

            if (worker == null)
            {
                return ApiResponse<string>.FailResponse("Worker not found");
            }

            if (worker.Status != WorkerStatus.Approved)
            {
                return ApiResponse<string>.FailResponse("Worker already approved");
            }

            worker.Status = WorkerStatus.Approved;
            worker.IsAvailable = true;

            await _workerRepository.UpdateAsync(worker);

            return ApiResponse<string>.SuccessResponse("Worker approved successfully", "OK");
        }
    }
}