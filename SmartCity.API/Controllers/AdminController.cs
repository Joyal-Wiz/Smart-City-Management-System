using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.API.Services;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Issues.Commands.AssignIssue;
using SmartCity.Application.Features.Issues.Commands.ReassignIssue;
using SmartCity.Application.Features.Issues.Queries.GetAllIssues;
using SmartCity.Application.Features.Sla.Queries.GetOverdueIssues;
using SmartCity.Application.Features.Sla.Queries.GetSlaSummary;
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
        private readonly NotificationRealtimeService _realtimeService;

        public AdminController(
            IMediator mediator,
            NotificationRealtimeService realtimeService)
        {
            _mediator = mediator;
            _realtimeService = realtimeService;
        }

        //  GET ALL WORKERS
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

        //  GET PENDING WORKERS
        [HttpGet("workers/pending")]
        public async Task<IActionResult> GetPendingWorkers()
        {
            var result = await _mediator.Send(new GetPendingWorkersQuery());

            return Ok(ApiResponse<object>.SuccessResponse("Pending workers fetched", result));
        }

        //  APPROVE WORKER
        [HttpPost("workers/approve")]
        public async Task<IActionResult> ApproveWorker([FromBody] ApproveWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Worker approved successfully", result));
        }

        //  REJECT WORKER
        [HttpPost("workers/reject")]
        public async Task<IActionResult> RejectWorker([FromBody] RejectWorkerCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse("Worker rejected successfully", result));
        }

        //  GET ALL ISSUES
        [HttpGet("issues")]
        public async Task<IActionResult> GetAllIssues([FromQuery] GetAllIssuesQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<object>.SuccessResponse("Issues fetched successfully", result));
        }

        //  SLA OVERDUE API
        [HttpGet("issues/overdue")]
        public async Task<IActionResult> GetOverdueIssues()
        {
            var result = await _mediator.Send(new GetOverdueIssuesQuery());

            return Ok(new
            {
                message = "Overdue issues fetched successfully",
                data = result
            });
        }

        //  ASSIGN ISSUE (FIXED REAL-TIME)
        [HttpPost("issues/assign")]
        public async Task<IActionResult> AssignIssue([FromBody] AssignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            //  REAL-TIME → WORKER
            await _realtimeService.SendAsync(
                command.WorkerId,
                $"New Work Assigned. Deadline: {command.Deadline}"
            );

            //  REAL-TIME → ADMIN
            await _realtimeService.SendAsync(
                null,
                $"Issue assigned to worker {command.WorkerId}"
            );

            return Ok(ApiResponse<object>.SuccessResponse("Issue assigned successfully", result));
        }

        [HttpGet("sla-summary")]
        public async Task<IActionResult> GetSlaSummary()
        {
            var result = await _mediator.Send(new GetSlaSummaryQuery());

            return Ok(new
            {
                message = "SLA summary fetched successfully",
                data = result
            });
        }

        //  REASSIGN ISSUE 
        [HttpPost("issues/reassign")]
        public async Task<IActionResult> ReassignIssue([FromBody] ReassignIssueCommand command)
        {
            var result = await _mediator.Send(command);

            await _realtimeService.SendAsync(
                command.WorkerId,
                "A new issue has been reassigned to you"
            );

            return Ok(ApiResponse<object>.SuccessResponse("Issue reassigned successfully", result));
        }
    }
}