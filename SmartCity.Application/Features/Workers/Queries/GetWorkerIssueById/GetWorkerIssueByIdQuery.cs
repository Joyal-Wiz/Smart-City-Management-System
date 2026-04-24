using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SmartCity.Application.Features.Workers.DTOs;

namespace SmartCity.Application.Features.Workers.Queries.GetWorkerIssueById
{

    public class GetWorkerIssueByIdQuery : IRequest<WorkerIssueDto>
    {
        public Guid Id { get; set; }
    }
}
