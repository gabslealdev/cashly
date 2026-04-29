using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Application.Identity.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<bool> ExistByEmailAsync(Email email);
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(Email email);
        Task<bool> ExistByIdAsync(Guid id);
    }
}
