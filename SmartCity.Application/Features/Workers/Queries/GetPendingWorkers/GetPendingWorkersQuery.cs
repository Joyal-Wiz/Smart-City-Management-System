using MediatR;
using SmartCity.Application.DTOs;



namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersQuery : IRequest<ApiResponse<PagedResult<WorkerDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}