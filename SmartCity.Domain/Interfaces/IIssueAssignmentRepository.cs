using SmartCity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCity.Domain.Interfaces
{
    public interface IIssueAssignmentRepository
    {
        Task AddAsync(IssueAssignment assignment);
        Task<int> GetActiveAssignmentsCount(Guid workerId);

    }
}
