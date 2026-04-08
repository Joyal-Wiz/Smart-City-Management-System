using SmartCity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCity.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

    }
}
