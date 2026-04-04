using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;

namespace SmartCity.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Issue> Issues { get; }
        DbSet<Worker> Workers { get; }
        DbSet<IssueAssignment> IssueAssignments { get; }
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<Notification> Notifications { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}