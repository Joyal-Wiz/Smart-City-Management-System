using Microsoft.AspNetCore.SignalR;
using SmartCity.API.Hubs;
using SmartCity.Application.Common.Interfaces;

namespace SmartCity.API.Services
{
    public class RealTimeService : IRealTimeService
    {
        private readonly IHubContext<IssueHub> _hubContext;

        public RealTimeService(IHubContext<IssueHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendIssueUpdated(Guid issueId, string status)
        {
            await _hubContext.Clients.All.SendAsync("IssueUpdated", new
            {
                issueId,
                status
            });
        }
    }
}