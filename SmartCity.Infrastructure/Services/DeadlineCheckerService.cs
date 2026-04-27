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

                    //   Real-time service
                    var realtimeService = scope.ServiceProvider.GetRequiredService<INotificationRealtimeService>();

                    var now = DateTime.UtcNow;

                    //  DEADLINE MISSED (SLA + REAL-TIME)
                    var expiredAssignments = await context.IssueAssignments
                        .Include(a => a.Issue)
                        .Include(a => a.Worker)
                        .Where(a =>
                            a.Deadline < now &&
                            a.Issue.Status != IssueStatus.Resolved &&
                            !a.IsDeadlineNotified)
                        .ToListAsync(stoppingToken);

                    foreach (var assignment in expiredAssignments)
                    {
                        _logger.LogWarning("Deadline missed for IssueId: {IssueId}", assignment.IssueId);

                        //  SLA STATE UPDATE
                        assignment.IsOverdue = true;
                        assignment.EscalationLevel = 1;
                        assignment.EscalatedAt = now;

                        var adminMessage = $"Issue {assignment.IssueId} was not completed before deadline";
                        var workerMessage = $"You missed the deadline for issue {assignment.IssueId}";

                        //  ADMIN NOTIFICATION (DB)
                        await notificationService.CreateAsync(
                            "Deadline Missed",
                            adminMessage,
                            "Issue",
                            assignment.IssueId
                        );

                        //  REAL-TIME → ADMIN
                        await realtimeService.SendAsync(null, adminMessage);

                        //  WORKER NOTIFICATION (DB)
                        await notificationService.CreateAsync(
                            "Deadline Missed",
                            workerMessage,
                            "Issue",
                            assignment.IssueId,
                            assignment.Worker.UserId
                        );

                        //  REAL-TIME → WORKER
                        await realtimeService.SendAsync(
                            assignment.Worker.UserId,
                            workerMessage
                        );

                        //  Prevent duplicate alerts
                        assignment.IsDeadlineNotified = true;
                    }

                    //  DEADLINE NEAR (10 HOURS BEFORE)
                    var nearDeadlineAssignments = await context.IssueAssignments
                        .Include(a => a.Issue)
                        .Include(a => a.Worker)
                        .Where(a =>
                            a.Deadline > now &&
                            a.Deadline <= now.AddHours(10) &&
                            a.Issue.Status != IssueStatus.Resolved &&
                            !a.IsDeadlineNotified)
                        .ToListAsync(stoppingToken);

                    foreach (var assignment in nearDeadlineAssignments)
                    {
                        var message = $"Your issue deadline is near: {assignment.Deadline}";

                        await notificationService.CreateAsync(
                            "Deadline Approaching",
                            message,
                            "Issue",
                            assignment.IssueId,
                            assignment.Worker.UserId
                        );

                        // REAL-TIME → WORKER
                        await realtimeService.SendAsync(
                            assignment.Worker.UserId,
                            message
                        );
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in DeadlineCheckerService");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}