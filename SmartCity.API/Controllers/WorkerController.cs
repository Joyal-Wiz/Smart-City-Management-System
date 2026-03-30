using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.Features.Issues.Commands.ResolveIssue;
using SmartCity.Application.Features.Issues.Commands.StartIssue;

namespace SmartCity.API.Controllers
{
    [Authorize(Roles = "Worker")]
    [ApiController]
    [Route("api/worker")]
    public class WorkerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = "Worker")]
        [HttpPost("issues/start")]
        public async Task<IActionResult> StartIssue([FromBody] StartIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Worker")]
        [HttpPost("issues/resolve")]
        public async Task<IActionResult> ResolveIssue([FromBody] ResolveIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}