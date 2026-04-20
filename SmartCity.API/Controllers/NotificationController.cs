using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Notifications.Commands.ClearNotifications;
using SmartCity.Application.Features.Notifications.Commands.MarkAllAsRead;
using SmartCity.Application.Features.Notifications.Commands.MarkAsRead;
using SmartCity.Application.Features.Notifications.Queries.GetMyNotifications;
using SmartCity.Application.Features.Notifications.Queries.GetUnreadCount;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize] //  all logged-in users
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET MY NOTIFICATIONS
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _mediator.Send(new GetMyNotificationsQuery());

            return Ok(ApiResponse<object>.SuccessResponse(
                "Notifications fetched",
                result
            ));
        }

        // UNREAD COUNT
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetUnreadCountQuery());

            return Ok(ApiResponse<object>.SuccessResponse(
                "Unread count",
                new { count }
            ));
        }

        //  MARK ALL AS READ
        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await _mediator.Send(new MarkAllAsReadCommand());

            return Ok(ApiResponse<object>.SuccessResponse(
                "All notifications marked as read"
            ));
        }

        //  MARK SINGLE
        [HttpPost("read")]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkNotificationAsReadCommand command)
        {
            await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse(
                "Notification marked as read"
            ));
        }
        // clear all
        [HttpPost("clear")]
        public async Task<IActionResult> ClearNotifications()
        {
            var result = await _mediator.Send(new ClearNotificationsCommand());

            return Ok(new
            {
                message = "Notifications cleared successfully",
                data = result
            });
        }
    }
}