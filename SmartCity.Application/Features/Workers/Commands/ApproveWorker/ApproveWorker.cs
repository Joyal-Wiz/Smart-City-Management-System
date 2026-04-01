using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Commands.ApproveWorker
{
    public class ApproveWorkerCommand : IRequest<ApiResponse<ApproveWorkerResponseDto>>
    {
        public Guid WorkerId { get; set; }
    }
}