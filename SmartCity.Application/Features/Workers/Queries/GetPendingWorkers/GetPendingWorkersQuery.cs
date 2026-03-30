using MediatR;
using SmartCity.Domain.Entities; 
using System.Collections.Generic;



namespace SmartCity.Application.Features.Workers.Queries.GetPendingWorkers
{
    public class GetPendingWorkersQuery : IRequest<List<Worker>>
    {
    }
}