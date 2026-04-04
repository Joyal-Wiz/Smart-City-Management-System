using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.AssignIssue;
using SmartCity.Application.Features.Issues.Commands.ReassignIssue;
using SmartCity.Application.Features.Issues.Queries.GetAllIssues;
using SmartCity.Application.Features.Workers.Commands.ApproveWorker;
using SmartCity.Application.Features.Workers.Commands.RejectWorker;
using SmartCity.Application.Features.Workers.Queries.GetAllWorkers;
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
        [HttpGet("workers")]
        public async Task<IActionResult> GetAllWorkers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var query = new GetAllWorkersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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
        [HttpPost("workers/reject")]
        public async Task<IActionResult> RejectWorker([FromBody] RejectWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("issues")]
        public async Task<IActionResult> GetAllIssues(
    [FromQuery] GetAllIssuesQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Issues fetched successfully",
                Data = result
            });
        }

        [HttpPost("issues/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignIssue([FromBody] AssignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("issues/reassign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReassignIssue([FromBody] ReassignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Issue reassigned successfully",
                Data = result
            });
        }
    }
}
