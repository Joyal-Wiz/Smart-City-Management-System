using MediatR;
using Microsoft.AspNetCore.Http;
using SmartCity.Domain.Enums;
using SmartCity.Application.Features.Issues.DTOs;

namespace SmartCity.Application.Features.Issues.Commands.CreateIssue
{
    public class CreateIssueCommand : IRequest<CreateIssueResponseDto>
    {
        public string Description { get; set; } = string.Empty;

        public IssueType Type { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public IFormFile? Image { get; set; }
    }
}