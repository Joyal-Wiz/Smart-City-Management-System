using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Queries.GetIssuesForMap
{
    public class GetIssuesForMapQueryHandler
        : IRequestHandler<GetIssuesForMapQuery, PagedResult<IssueMapDto>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ICurrentUserService _currentUser;

        public GetIssuesForMapQueryHandler(
            IIssueRepository issueRepository,
            ICurrentUserService currentUser)
        {
            _issueRepository = issueRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<IssueMapDto>> Handle(
            GetIssuesForMapQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var role = _currentUser.Role;

            var (items, totalCount) = await _issueRepository.GetIssuesForMapAsync(
                request.Latitude,
                request.Longitude,
                request.RadiusKm,
                request.Status,
                userId,
                role,
                request.PageNumber,
                request.PageSize
            );

            return new PagedResult<IssueMapDto>
            {
                Items = items.Select(i => new IssueMapDto
                {
                    Id = i.Id,
                    Latitude = i.Location.Latitude,
                    Longitude = i.Location.Longitude,
                    Status = i.Status.ToString(),
                    Title = i.Description
                }).ToList(),

                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}