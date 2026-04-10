using MediatR;
using SmartCity.Application.DTOs.Sla;

namespace SmartCity.Application.Features.Sla.Queries.GetOverdueIssues
{
    public class GetOverdueIssuesQuery : IRequest<List<OverdueIssueDto>>
    {
    }
}