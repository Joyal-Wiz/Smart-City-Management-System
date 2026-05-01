namespace SmartCity.Application.Common.Interfaces
{
    public interface IRealTimeService
    {
        Task SendIssueUpdated(Guid issueId, string status);
    }
}