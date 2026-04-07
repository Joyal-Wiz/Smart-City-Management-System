using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.AssignIssue;
using SmartCity.Application.Features.Issues.Commands.ReassignIssue;
using SmartCity.Application.Features.Issues.Queries.GetAllIssues;
using SmartCity.Application.Features.Notifications.Commands.MarkAdminNotificationsAsRead;
using SmartCity.Application.Features.Notifications.Commands.MarkAsRead;
using SmartCity.Application.Features.Notifications.Queries.GetAdminNotifications;
using SmartCity.Application.Features.Notifications.Queries.GetAdminUnreadCount;
using SmartCity.Application.Features.Workers.Commands.ApproveWorker;
using SmartCity.Application.Features.Workers.Commands.RejectWorker;
using SmartCity.Application.Features.Workers.Queries.GetAllWorkers;
using SmartCity.Application.Features.Workers.Queries.GetPendingWorkers;

namespace SmartCity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔔 GET ADMIN NOTIFICATIONS
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _mediator.Send(new GetAdminNotificationsQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Notifications fetched", result));
        }

        // 🔔 UNREAD COUNT
        [HttpGet("notifications/unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetAdminUnreadCountQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Unread count", new { count }));
        }

        // 🔔 MARK ALL AS READ (ADMIN)
        [HttpPost("notifications/read-all")]
        public async Task<IActionResult> MarkAdminNotificationsAsRead()
        {
            await _mediator.Send(new MarkAdminNotificationsAsReadCommand());

            return Ok(ApiResponse<object>.SuccessResponse("Admin notifications marked as read"));
        }

        // 🔔 MARK SINGLE NOTIFICATION
        [HttpPost("notifications/read")]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkNotificationAsReadCommand command)
        {
            await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Notification marked as read"));
        }

        // 👷 GET ALL WORKERS (PAGINATED)
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

            return Ok(ApiResponse<object>.SuccessResponse("Workers fetched successfully", result));
        }

        // 👷 GET PENDING WORKERS
        [HttpGet("workers/pending")]
        public async Task<IActionResult> GetPendingWorkers()
        {
            var result = await _mediator.Send(new GetPendingWorkersQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Pending workers fetched", result));
        }

        // 👷 APPROVE WORKER
        [HttpPost("workers/approve")]
        public async Task<IActionResult> ApproveWorker([FromBody] ApproveWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Worker approved successfully", result));
        }

        // 👷 REJECT WORKER
        [HttpPost("workers/reject")]
        public async Task<IActionResult> RejectWorker([FromBody] RejectWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Worker rejected successfully", result));
        }

        // 🧾 GET ALL ISSUES
        [HttpGet("issues")]
        public async Task<IActionResult> GetAllIssues([FromQuery] GetAllIssuesQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<object>.SuccessResponse("Issues fetched successfully", result));
        }

        // 🧾 ASSIGN ISSUE
        [HttpPost("issues/assign")]
        public async Task<IActionResult> AssignIssue([FromBody] AssignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Issue assigned successfully", result));
        }

        // 🧾 REASSIGN ISSUE
        [HttpPost("issues/reassign")]
        public async Task<IActionResult> ReassignIssue([FromBody] ReassignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Issue reassigned successfully", result));
        }
    }
}