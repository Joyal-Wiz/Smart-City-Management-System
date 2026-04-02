using MediatR;
using Microsoft.AspNetCore.Http;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.ResolveIssue
{
    public class ResolveIssueCommand : IRequest<ApiResponse<string>>
    {
        public Guid IssueId { get; set; }

        public IFormFile Image { get; set; }
    }
}