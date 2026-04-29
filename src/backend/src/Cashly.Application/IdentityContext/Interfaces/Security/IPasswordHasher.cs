using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Application.IdentityContext.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, PasswordHash passwordHash);
    }
}
