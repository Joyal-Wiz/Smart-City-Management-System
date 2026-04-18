using MediatR;
using SmartCity.Application.Features.Issues.DTOs;

namespace SmartCity.Application.Features.Issues.Queries.GetIssueById
{
    public record GetIssueByIdQuery(Guid Id) : IRequest<AdminIssueDetailsDto>;
}