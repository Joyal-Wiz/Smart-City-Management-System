using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Issue>()
                .OwnsOne(i => i.Location);

            modelBuilder.Entity<IssueAssignment>()
                .Property(x => x.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<IssueAssignment>()
                .HasOne<Issue>() 
                .WithMany()      
                .HasForeignKey(x => x.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IssueAssignment>()
                .HasIndex(x => x.IssueId);

            modelBuilder.Entity<IssueAssignment>()
                .HasIndex(x => x.WorkerId);
        }
    }
}