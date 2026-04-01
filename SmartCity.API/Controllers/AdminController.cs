using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.Features.Issues.Commands.AssignIssue;
using SmartCity.Application.Features.Workers.Commands.ApproveWorker;
using SmartCity.Application.Features.Workers.Commands.RejectWorker;
using SmartCity.Application.Features.Workers.Queries.GetPendingWorkers;

namespace SmartCity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")] 
        [HttpGet("workers/pending")]
        public async Task<IActionResult> GetPendingWorkers([FromQuery] GetPendingWorkersQuery query)
        {
            var result = await _mediator.Send(new GetPendingWorkersQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("workers/approve")]
        public async Task<IActionResult> ApproveWorker([FromBody] ApproveWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reject")]
        public async Task<IActionResult> RejectWorker([FromBody] RejectWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("issues/assign")]
        public async Task<IActionResult> AssignIssue([FromBody] AssignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
