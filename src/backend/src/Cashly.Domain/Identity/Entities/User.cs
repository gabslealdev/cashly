using Cashly.Domain.Identity.ValueObjects;
using Cashly.Domain.Shared.Entities;

namespace Cashly.Domain.Identity.Entities
{
    public sealed class User : Entity
    {
        public Name Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public PasswordHash PasswordHash { get; private set; } = null!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private User() { }

        private User(Name name, Email email, PasswordHash passwordHash)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static User Create(Name name, Email email, PasswordHash password)
            => new User(name, email, password);

        public void UpdateName(Name name)
        {
            Name = name;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateEmail(Email email)
        {
            Email = email;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

    }
}
