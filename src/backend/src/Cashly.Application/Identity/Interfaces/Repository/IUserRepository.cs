using Cashly.Domain.Identity.Entities;
using Cashly.Domain.Identity.ValueObjects;

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
