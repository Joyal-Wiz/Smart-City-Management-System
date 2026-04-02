using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;
using SmartCity.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCity.Infrastructure.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly AppDbContext _context;

        public WorkerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Worker>> GetPendingWorkersAsync()
        {
            return await _context.Workers
                .Where(w => w.Status == WorkerStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<Worker>> GetAvailableWorkersAsync()
        {
            return await _context.Workers
                .Where(w => w.IsAvailable && w.Status == WorkerStatus.Approved)
                .ToListAsync();
        }

        public async Task<Worker> GetByIdAsync(Guid id)
        {
            return await _context.Workers.FindAsync(id);
        }
        public async Task<Worker?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Workers
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task UpdateAsync(Worker worker)
        {
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Worker worker)
        {
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Worker>> GetAllAsync()
        {
            return await _context.Workers.ToListAsync();
        }

        public async Task<(List<Worker>, int)> GetPendingWorkersPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Workers
                .Include(w => w.User) 
                .Where(w => w.Status == WorkerStatus.Pending);

            var totalCount = await query.CountAsync();

            var workers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (workers, totalCount);
        }
        public IQueryable<Worker> GetQueryable()
        {
            return _context.Workers.AsQueryable();
        }
    }
}