using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Domain.Identity.Entities;
using Cashly.Domain.Identity.ValueObjects;
using Cashly.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Repositories.Identity
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly CashlyDbContext _context;

        public UserRepository(CashlyDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistByEmailAsync(Email email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(Email email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistByIdAsync(Guid id)
        {
            return await _context.Users.AnyAsync(x => x.Id == id);
        }
    }
}
