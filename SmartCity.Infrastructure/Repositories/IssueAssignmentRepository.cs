using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;
using SmartCity.Infrastructure.Persistence;

namespace SmartCity.Infrastructure.Repositories
{
    public class IssueAssignmentRepository : IIssueAssignmentRepository
    {
        private readonly AppDbContext _context;

        public IssueAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IssueAssignment assignment)
        {
            await _context.IssueAssignments.AddAsync(assignment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<int> GetActiveAssignmentsCount(Guid workerId)
        {
            return await _context.IssueAssignments
                .Include(a => a.Issue)
                .Where(a => a.WorkerId == workerId &&
                            a.Issue.Status != IssueStatus.Resolved)
                .CountAsync();
        }
    }
}