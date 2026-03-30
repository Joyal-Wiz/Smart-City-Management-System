using MediatR;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersHandler : IRequestHandler<GetPendingWorkersQuery, List<Worker>>
    {
        private readonly IWorkerRepository _workerRepository;

        public GetPendingWorkersHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<List<Worker>> Handle(GetPendingWorkersQuery request, CancellationToken cancellationToken)
        {
            return await _workerRepository.GetPendingWorkersAsync();
        }
    }
}