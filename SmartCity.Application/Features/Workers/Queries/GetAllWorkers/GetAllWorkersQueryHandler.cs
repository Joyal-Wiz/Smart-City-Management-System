using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Queries.GetAllWorkers
{
    public class GetAllWorkersQueryHandler
        : IRequestHandler<GetAllWorkersQuery, PagedResult<WorkerDto>>
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IIssueAssignmentRepository _assignmentRepository;

        public GetAllWorkersQueryHandler(
            IWorkerRepository workerRepository,
            IIssueAssignmentRepository assignmentRepository)
        {
            _workerRepository = workerRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<PagedResult<WorkerDto>> Handle(
            GetAllWorkersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _workerRepository
     .GetQueryable()
     .Include(w => w.User)
     .Where(w => w.Status == WorkerStatus.Approved);

            var totalCount = await query.CountAsync(cancellationToken);

            var workers = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var workerDtos = new List<WorkerDto>();

            foreach (var w in workers)
            {
                var activeCount = await _assignmentRepository.GetActiveAssignmentsCount(w.Id);

                workerDtos.Add(new WorkerDto
                {
                    Id = w.Id,
                    Name = w.User.Name,
                    Email = w.User.Email,
                    Status = w.Status,
                    IsAvailable = activeCount < 4 // 🔥 FINAL LOGIC
                });
            }

            return new PagedResult<WorkerDto>
            {
                Items = workerDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}