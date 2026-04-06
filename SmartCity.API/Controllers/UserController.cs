using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Queries.GetMyIssueDetails;
using SmartCity.Application.Features.Issues.Queries.GetMyIssuesList;
using SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead;
using SmartCity.Application.Features.Notifications.Queries.GetMyNotifications;
using SmartCity.Application.Features.Notifications.Queries.GetUnreadCount;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "Citizen")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔹 ISSUE DETAILS
        [HttpGet("issues/{id}")]
        public async Task<IActionResult> GetIssueDetails(Guid id)
        {
            var result = await _mediator.Send(new GetMyIssueDetailsQuery(id));
            return Ok(result);
        }

        // 🔹 ISSUE LIST (NEW)
        [HttpGet("issues")]
        public async Task<IActionResult> GetMyIssues(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetMyIssuesListQuery(pageNumber, pageSize));

            return Ok(result);
        }

        // 🔔 GET USER NOTIFICATIONS
        [HttpGet("notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _mediator.Send(new GetMyNotificationsQuery());

            return Ok(ApiResponse<object>.SuccessResponse(
                "Notifications fetched successfully",
                result
            ));
        }

        // 🔔 UNREAD COUNT
        [HttpGet("notifications/unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetUnreadCountQuery());

            return Ok(ApiResponse<object>.SuccessResponse(
                "Unread count fetched",
                new { count }
            ));
        }

        // 🔔 MARK ALL AS READ
        [HttpPost("notifications/read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _mediator.Send(new MarkAllAsReadCommand());

            return Ok(ApiResponse<object>.SuccessResponse(
                "All notifications marked as read",
                result
            ));
        }
    }
}