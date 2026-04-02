using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Workers.DTOs;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Workers.Queries.GetAllWorkers
{
    public class GetAllWorkersQueryHandler
        : IRequestHandler<GetAllWorkersQuery, ApiResponse<PagedResult<WorkerDto>>>
    {
        private readonly IWorkerRepository _workerRepository;

        public GetAllWorkersQueryHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<PagedResult<WorkerDto>>> Handle(
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
                .Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.User.Name,
                    Email = w.User.Email,
                    Status = w.Status
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<WorkerDto>
            {
                Items = workers,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return ApiResponse<PagedResult<WorkerDto>>.SuccessResponse(
                "Workers fetched successfully",
                pagedResult
            );
        }
    }
}