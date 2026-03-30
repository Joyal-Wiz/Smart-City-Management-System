using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersHandler : IRequestHandler<GetPendingWorkersQuery, List<WorkerDto>>
    {
        private readonly IWorkerRepository _workerRepository;

        public GetPendingWorkersHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<List<WorkerDto>> Handle(GetPendingWorkersQuery request, CancellationToken cancellationToken)
        {
            var workers = await _workerRepository.GetPendingWorkersAsync();

            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                IsAvailable = w.IsAvailable,
                Status = w.Status.ToString()
            }).ToList();
        }
    }
}