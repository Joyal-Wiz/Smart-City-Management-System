using MediatR;
using Microsoft.AspNetCore.Http;
using SmartCity.Application.DTOs;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Issues.Commands.CreateIssue
{
    public class CreateIssueCommand : IRequest<ApiResponse<Guid>>
    {
        public string Description { get; set; } = string.Empty;

        public IssueType Type { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public IFormFile? Image { get; set; }
    }
}