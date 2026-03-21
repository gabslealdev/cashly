using Cashly.Domain.Identity.ValueObjects;

namespace Cashly.Application.Identity.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, PasswordHash passwordHash);
    }
}
