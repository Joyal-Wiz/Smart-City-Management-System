using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.CreateIssue;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/issues")]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🧾 CREATE ISSUE (Citizen Only)
        [HttpPost]
        [Authorize(Roles = "Citizen")]
        public async Task<IActionResult> CreateIssue([FromForm] CreateIssueCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.FailResponse("Invalid request"));
            }

            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse(
                "Issue created successfully",
                result
            ));
        }
    }
}