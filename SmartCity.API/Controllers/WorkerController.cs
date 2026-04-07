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

        // 🔔 GET NOTIFICATIONS
        [HttpGet("notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _mediator.Send(new GetMyNotificationsQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Notifications fetched", result));
        }

        // 🔔 UNREAD COUNT
        [HttpGet("notifications/unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetUnreadCountQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Unread count", new { count }));
        }

        // 🔔 MARK ALL AS READ
        [HttpPost("notifications/read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await _mediator.Send(new MarkAllAsReadCommand());

            return Ok(ApiResponse<object>.SuccessResponse("All notifications marked as read"));
        }

        // 🔧 GET MY ISSUES
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

            return Ok(ApiResponse<object>.SuccessResponse("Worker issues fetched", result));
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