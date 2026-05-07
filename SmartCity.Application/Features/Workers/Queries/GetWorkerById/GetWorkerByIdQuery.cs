using MediatR;
using SmartCity.Application.Features.Workers.DTO;
using System;

namespace SmartCity.Application.Features.Workers.Queries.GetWorkerById
{
    public class GetWorkerByIdQuery : IRequest<GetWorkerByIdResponseDto>
    {
        public Guid Id { get; set; }

        public GetWorkerByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
