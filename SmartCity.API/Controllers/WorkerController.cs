using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.RejectIssue;
using SmartCity.Application.Features.Issues.Commands.ResolveIssue;
using SmartCity.Application.Features.Issues.Commands.StartIssue;
using SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead;
using SmartCity.Application.Features.Notifications.Queries.GetMyNotifications;
using SmartCity.Application.Features.Notifications.Queries.GetUnreadCount;
using SmartCity.Application.Features.Workers.Queries.GetMyIssues;
using SmartCity.Application.Features.Workers.Queries.GetWorkerIssueById;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/worker")]
    [Authorize(Roles = "Worker")]
    public class WorkerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // 🔧 GET MY ISSUES
        [HttpGet("issues")]
        public async Task<IActionResult> GetMyIssues(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? status = null) // 🔥 NEW
        {
            var query = new GetMyIssuesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status
            };

            var result = await _mediator.Send(query);

            return Ok(ApiResponse<object>.SuccessResponse("Worker issues fetched", result));
        }

        [HttpGet("issues/{id}")]
        public async Task<IActionResult> GetIssueById(Guid id)
        {
            var result = await _mediator.Send(new GetWorkerIssueByIdQuery { Id = id });

            return Ok(ApiResponse<object>.SuccessResponse("Issue fetched", result));
        }

        // ▶️ START ISSUE
        [HttpPost("issues/start")]
        public async Task<IActionResult> StartIssue([FromBody] StartIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Issue started successfully", result));
        }

        // ✅ RESOLVE ISSUE
        [HttpPost("issues/resolve")]
        public async Task<IActionResult> ResolveIssue([FromForm] ResolveIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Issue resolved successfully", result));
        }

        // ❌ REJECT ISSUE
        [HttpPost("issues/reject")]
        public async Task<IActionResult> RejectIssue([FromBody] RejectIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Issue rejected successfully", result));
        }
    }
}