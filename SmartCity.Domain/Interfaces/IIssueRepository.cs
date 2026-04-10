using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;

namespace SmartCity.Domain.Interfaces
{
    public interface IIssueRepository
    {
        // 🔹 Create
        Task AddAsync(Issue issue);

        // 🔹 Read (Single)
        Task<Issue?> GetByIdAsync(Guid id);

        // 🔹 Read (All)
        Task<List<Issue>> GetAllAsync();

        // 🔹 Update
        Task UpdateAsync(Issue issue);

        Task<(List<Issue> Items, int TotalCount)> GetIssuesForMapAsync(
            double latitude,
            double longitude,
            double radiusKm,
            IssueStatus? status,
            Guid userId,
            string role,
            int pageNumber,
            int pageSize
        );
    }
}