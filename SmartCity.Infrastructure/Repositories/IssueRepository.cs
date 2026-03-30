using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;
using SmartCity.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Issue>> GetAllAsync()
        {
            return await _context.Issues.ToListAsync();
        }

        public async Task<Issue?> GetByIdAsync(Guid id)
        {
            return await _context.Issues.FindAsync(id);
        }

        public async Task UpdateAsync(Issue issue)
        {
            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();
        }
    }
}
