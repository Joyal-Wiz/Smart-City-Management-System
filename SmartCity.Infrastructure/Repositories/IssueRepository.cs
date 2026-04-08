using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;
using SmartCity.Infrastructure.Persistence;

namespace SmartCity.Infrastructure.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Issue issue)
        {
            await _context.Issues.AddAsync(issue);
            await _context.SaveChangesAsync();
        }

        public async Task<Issue?> GetByIdAsync(Guid id)
        {
            return await _context.Issues.FindAsync(id);
        }

        public async Task<List<Issue>> GetAllAsync()
        {
            return await _context.Issues.ToListAsync();
        }

        public async Task UpdateAsync(Issue issue)
        {
            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<Issue> Items, int TotalCount)> GetIssuesForMapAsync(
            double latitude,
            double longitude,
            double radiusKm,
            IssueStatus? status,
            Guid userId,
            string role,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Issues.AsQueryable();

            // 🔥 Haversine
            query = query.Where(i =>
                6371 * Math.Acos(
                    Math.Cos(Math.PI * latitude / 180) *
                    Math.Cos(Math.PI * i.Location.Latitude / 180) *
                    Math.Cos(Math.PI * (i.Location.Longitude - longitude) / 180) +
                    Math.Sin(Math.PI * latitude / 180) *
                    Math.Sin(Math.PI * i.Location.Latitude / 180)
                ) <= radiusKm
            );

            // Status filter
            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            // Role filter
            if (role == "Worker")
            {
                query = query.Where(i => i.AssignedWorkerId == userId);
            }
            else if (role == "Citizen")
            {
                query = query.Where(i => i.CreatedByUserId == userId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}