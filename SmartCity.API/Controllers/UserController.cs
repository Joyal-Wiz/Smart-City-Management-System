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

    }
}