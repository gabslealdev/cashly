using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Application.IdentityContext.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<bool> ExistByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> ExistByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
