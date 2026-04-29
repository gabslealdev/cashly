using Bogus;
using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Domain.UnitTests.Identity.Builders
{
    public sealed class UserBuilder
    {
        private readonly Faker _faker = new();
        private Name? _name;
        private Email? _email;

        public UserBuilder WithName(Name name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithEmail(Email email)
        {
            _email = email;
            return this;
        }

        public User Build()
        {
            var name = _name ?? Name.Create(_faker.Name.FirstName(), _faker.Name.LastName());
            var email = _email ?? Email.Create(_faker.Internet.Email());
            var passwordHash = PasswordHash.Create(_faker.Random.Hash());

            return User.Create(name, email, passwordHash);
        }
    }
}
