using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
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

        // ✅ Get Pending Workers
        public async Task<List<Worker>> GetPendingWorkersAsync()
        {
            return await _context.Workers
                .Where(w => w.Status == "Pending")
                .ToListAsync();
        }

        // ✅ Get By Id
        public async Task<Worker> GetByIdAsync(Guid id)
        {
            return await _context.Workers.FindAsync(id);
        }

        // ✅ Update Worker
        public async Task UpdateAsync(Worker worker)
        {
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
        }

        // ✅ Get Available Workers
        public async Task<List<Worker>> GetAvailableWorkersAsync()
        {
            return await _context.Workers
                .Where(w => w.IsAvailable && w.Status == "Approved")
                .ToListAsync();
        }
        public async Task AddAsync(Worker worker)
        {
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
        }
    }
}