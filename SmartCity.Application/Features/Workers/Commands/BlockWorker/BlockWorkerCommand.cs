using MediatR;
using System;

namespace SmartCity.Application.Features.Workers.Commands.BlockWorker
{
    public class BlockWorkerCommand : IRequest<bool>
    {
        public Guid WorkerId { get; set; }
        public string Reason { get; set; }

        public BlockWorkerCommand(Guid workerId, string reason)
        {
            WorkerId = workerId;
            Reason = reason;
        }
    }
}
