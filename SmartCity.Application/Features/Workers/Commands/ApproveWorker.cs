using MediatR;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Workers.Commands.ApproveWorker
{
    public class ApproveWorkerCommand : IRequest<ApiResponse<string>>
    {
        public Guid WorkerId { get; set; }
    }
}