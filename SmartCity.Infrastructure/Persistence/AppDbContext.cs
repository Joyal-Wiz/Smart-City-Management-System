using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCity.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Value Object configuration
            modelBuilder.Entity<Issue>()
                .OwnsOne(i => i.Location);
        }
    }
}
