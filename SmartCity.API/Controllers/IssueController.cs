using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.CreateIssue;
using SmartCity.Application.Features.Issues.Queries.GetIssuesForMap;
using SmartCity.Domain.Enums;

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

        //  CREATE ISSUE (Citizen Only)
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

        //  MAP FEATURE — GET ISSUES BY LOCATION
        [HttpGet("map")]
        public async Task<IActionResult> GetIssuesForMap(
            [FromQuery] double lat,
            [FromQuery] double lng,
            [FromQuery] double radiusKm = 5,
            [FromQuery] IssueStatus? status = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetIssuesForMapQuery
            {
                Latitude = lat,
                Longitude = lng,
                RadiusKm = radiusKm,
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(ApiResponse<PagedResult<IssueMapDto>>.SuccessResponse(
                "Map issues fetched successfully",
                result
            ));
        }
    }
}