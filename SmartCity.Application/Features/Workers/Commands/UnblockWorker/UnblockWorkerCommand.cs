using MediatR;
using System;

namespace SmartCity.Application.Features.Workers.Commands.UnblockWorker
{
    public class UnblockWorkerCommand : IRequest<bool>
    {
        public Guid WorkerId { get; set; }

        public UnblockWorkerCommand(Guid workerId)
        {
            WorkerId = workerId;
        }
    }
}
