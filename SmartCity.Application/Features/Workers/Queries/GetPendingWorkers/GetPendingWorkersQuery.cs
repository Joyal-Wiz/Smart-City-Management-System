using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;



namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersQuery : IRequest<PagedResult<WorkerDto>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}