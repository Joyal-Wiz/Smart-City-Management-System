using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.Features.Issues.Commands.CreateIssue;

namespace SmartCity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/issues")]
    public class IssueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Citizen")]
        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromForm] CreateIssueCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}