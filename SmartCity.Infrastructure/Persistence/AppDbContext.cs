using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;

namespace SmartCity.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<IssueAssignment> IssueAssignments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Value Object (Location)
            modelBuilder.Entity<Issue>()
                .OwnsOne(i => i.Location);

            // 🔹 Salary precision
            modelBuilder.Entity<IssueAssignment>()
                .Property(x => x.Salary)
                .HasPrecision(18, 2);

            // 🔹 IssueAssignment → Issue relationship
            modelBuilder.Entity<IssueAssignment>()
                .HasOne<Issue>()
                .WithMany()
                .HasForeignKey(x => x.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Indexes
            modelBuilder.Entity<IssueAssignment>()
                .HasIndex(x => x.IssueId);

            modelBuilder.Entity<IssueAssignment>()
                .HasIndex(x => x.WorkerId);

            // 🔹 Worker → User relationship
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IssueAssignment>()
                .HasOne(ia => ia.Issue)
                .WithMany(i => i.Assignments)
                .HasForeignKey(ia => ia.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IssueAssignment>()
                .HasOne(ia => ia.Worker)
                .WithMany(w => w.Assignments)
                .HasForeignKey(ia => ia.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);          
        }
    }
}