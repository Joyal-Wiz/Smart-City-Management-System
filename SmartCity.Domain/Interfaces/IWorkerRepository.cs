using SmartCity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCity.Domain.Interfaces
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAvailableWorkersAsync();

        Task<Worker?> GetByIdAsync(Guid id);

        Task UpdateAsync(Worker worker);
    }
}
