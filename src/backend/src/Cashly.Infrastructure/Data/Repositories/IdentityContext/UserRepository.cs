using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;
using Cashly.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Repositories.IdentityContext
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly CashlyDbContext _context;

        public UserRepository(CashlyDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task<bool> ExistByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> ExistByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
