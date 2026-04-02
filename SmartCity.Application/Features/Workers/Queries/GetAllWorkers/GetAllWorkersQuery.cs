using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Queries.GetAllWorkers
{
    public class GetAllWorkersQuery : IRequest<ApiResponse<PagedResult<WorkerDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}