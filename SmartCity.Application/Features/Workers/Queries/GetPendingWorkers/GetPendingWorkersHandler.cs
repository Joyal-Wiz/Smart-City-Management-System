using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersHandler
        : IRequestHandler<GetPendingWorkersQuery,PagedResult<WorkerDto>>
    {
        private readonly IWorkerRepository _workerRepository;

        public GetPendingWorkersHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<PagedResult<WorkerDto>> Handle(
            GetPendingWorkersQuery request,
            CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var (workers, totalCount) = await _workerRepository
                .GetPendingWorkersPagedAsync(pageNumber, pageSize);

            var workerDtos = workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.User.Name,
                Email = w.User.Email,
                Status = w.Status,
                IsAvailable = w.IsAvailable
            }).ToList();

            var result = new PagedResult<WorkerDto>
            {
                Items = workerDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return result;
        }
    }
}