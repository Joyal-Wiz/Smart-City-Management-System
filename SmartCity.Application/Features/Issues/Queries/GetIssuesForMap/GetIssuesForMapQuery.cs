using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Domain.Enums;

public class GetIssuesForMapQuery : IRequest<PagedResult<IssueMapDto>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusKm { get; set; } = 5;

    public IssueStatus? Status { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}