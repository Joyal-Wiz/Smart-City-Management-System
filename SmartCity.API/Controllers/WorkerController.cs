using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.RejectIssue;
using SmartCity.Application.Features.Issues.Commands.ResolveIssue;
using SmartCity.Application.Features.Issues.Commands.StartIssue;
using SmartCity.Application.Features.Workers.Queries.GetAllWorkers;
using SmartCity.Application.Features.Workers.Queries.GetMyIssues;

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
        [HttpGet("issues")]
        public async Task<IActionResult> GetMyIssues(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetMyIssuesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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
        public async Task<IActionResult> ResolveIssue([FromForm] ResolveIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("issues/reject")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> RejectIssue(RejectIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Issue marked as rejected",
                Data = result
            });
        }
    }
}