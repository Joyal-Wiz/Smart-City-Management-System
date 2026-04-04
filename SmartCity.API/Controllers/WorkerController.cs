using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.RejectIssue;
using SmartCity.Application.Features.Issues.Commands.ResolveIssue;
using SmartCity.Application.Features.Issues.Commands.StartIssue;
using SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead;
using SmartCity.Application.Features.Notifications.DTOs; // 🔥 DTO
using SmartCity.Application.Features.Notifications.Queries.GetMyNotifications;
using SmartCity.Application.Features.Notifications.Queries.GetUnreadCount;
using SmartCity.Application.Features.Workers.Queries.GetMyIssues;
using SmartCity.Application.Interfaces;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/worker")]
    [Authorize(Roles = "Worker")]
    public class WorkerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public WorkerController(
            IMediator mediator,
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _context = context;
            _currentUser = currentUser;
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _mediator.Send(new GetMyNotificationsQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Notifications fetched", result));
        }

        [HttpGet("notifications/unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetUnreadCountQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Unread count", new { count }));
        }

        [HttpPost("notifications/read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _mediator.Send(new MarkAllAsReadCommand());

            return Ok(ApiResponse<object>.SuccessResponse("All notifications marked as read", result));
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

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ▶️ START ISSUE
        [HttpPost("issues/start")]
        public async Task<IActionResult> StartIssue([FromBody] StartIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ✅ RESOLVE ISSUE
        [HttpPost("issues/resolve")]
        public async Task<IActionResult> ResolveIssue([FromForm] ResolveIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ❌ REJECT ISSUE
        [HttpPost("issues/reject")]
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