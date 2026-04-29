using Bogus;
using Cashly.Domain.IdentityContext.Errors;
using Cashly.Domain.IdentityContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Shouldly;

namespace Cashly.Domain.UnitTests.IdentityContext.ValueObjects
{
    public class PasswordHashUnitTest
    {
        private readonly Faker _faker = new();

        [Fact]
        public void CreatePasswordHash_ShouldCreate_WhenValueIsValid()
        {
            // arrange 
            var passwordHash = _faker.Random.Hash();

            // act
            Action action = () => PasswordHash.Create(passwordHash);

            // assert
            action.ShouldNotThrow();
        }

        [Fact]
        public void CreatePasswordHash_ShouldThrow_WhenValueIsNullOrEmpty()
        {
            // arrange 
            var passwordHash = string.Empty;

            // act
            Action action = () => PasswordHash.Create(passwordHash);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(UserErrors.PasswordRequired.Code);
            exception.Error.Message.ShouldBe(UserErrors.PasswordRequired.Message);

        }
    }
}
