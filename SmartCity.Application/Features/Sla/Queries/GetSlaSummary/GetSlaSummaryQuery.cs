using MediatR;
using SmartCity.Application.DTOs.Sla;

namespace SmartCity.Application.Features.Sla.Queries.GetSlaSummary
{
    public class GetSlaSummaryQuery : IRequest<SlaSummaryDto>
    {
    }
}