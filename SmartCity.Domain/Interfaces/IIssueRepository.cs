using System;
using System.Collections.Generic;
using System.Text;
using SmartCity.Domain.Entities;

namespace SmartCity.Domain.Interfaces
{

    public interface IIssueRepository
    {
        Task AddAsync(Issue issue);

        Task<Issue?> GetByIdAsync(Guid id);

        Task<List<Issue>> GetAllAsync();

        Task UpdateAsync(Issue issue);
    }
}
