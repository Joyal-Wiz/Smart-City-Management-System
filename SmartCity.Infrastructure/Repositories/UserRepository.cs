using Microsoft.EntityFrameworkCore;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;
using SmartCity.Infrastructure.Persistence;

namespace SmartCity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository   // ✅ CORRECT INTERFACE
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Add User
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // ✅ Get by Email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // ✅ Get by Id
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}