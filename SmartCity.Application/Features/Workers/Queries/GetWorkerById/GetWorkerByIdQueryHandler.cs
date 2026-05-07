using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Features.Workers.DTO;
using SmartCity.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCity.Application.Features.Workers.Queries.GetWorkerById
{
    public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, GetWorkerByIdResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetWorkerByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetWorkerByIdResponseDto> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = await _context.Workers
                .Include(w => w.User)
                .Include(w => w.Assignments)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (worker == null)
                return null;

            return new GetWorkerByIdResponseDto
            {
                Id = worker.Id,
                Name = worker.User.Name,
                Email = worker.User.Email,
                PhoneNumber = worker.User.PhoneNumber,
                Status = worker.Status,
                IsAvailable = worker.IsAvailable,
                CreatedAt = worker.CreatedAt,
                TotalIssuesAssigned = worker.Assignments.Count,
                BlockReason = worker.BlockReason
            };
        }
    }
}
