using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Commands.RejectWorker
{
    public class RejectWorkerCommand : IRequest<ApiResponse<RejectWorkerResponseDto>>
    {
        public Guid WorkerId { get; set; }
    }
}