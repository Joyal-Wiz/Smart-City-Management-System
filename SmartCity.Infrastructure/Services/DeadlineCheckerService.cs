using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Infrastructure.Services
{
    public class DeadlineCheckerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DeadlineCheckerService> _logger;

        public DeadlineCheckerService(
            IServiceScopeFactory scopeFactory,
            ILogger<DeadlineCheckerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Deadline Checker Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var now = DateTime.UtcNow;

                    var expiredIssues = await context.IssueAssignments
                        .Include(a => a.Issue)
                        .Where(a =>
                            a.Deadline < now &&
                            a.Issue.Status != IssueStatus.Resolved &&
                            a.IsDeadlineNotified == false) // 🔥 PREVENT DUPLICATE
                        .ToListAsync(stoppingToken);

                    foreach (var assignment in expiredIssues)
                    {
                        _logger.LogWarning("Deadline missed for IssueId: {IssueId}", assignment.IssueId);

                        await notificationService.CreateAsync(
                            "Deadline Missed",
                            $"Issue {assignment.IssueId} was not completed before deadline",
                            "Issue",
                            assignment.IssueId
                        );

                        // 🔥 MARK AS NOTIFIED
                        assignment.IsDeadlineNotified = true;
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in DeadlineCheckerService");
                }

                // 🔥 Runs every 2 minutes
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}