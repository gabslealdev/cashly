using Bogus;
using Cashly.Domain.Identity.Errors;
using Cashly.Domain.Identity.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Shouldly;

namespace Cashly.Domain.UnitTests.Identity.ValueObjects
{
    public class EmailUnitTest
    {
        private readonly Faker _faker = new();

        [Fact]
        public void CreateEmail_ShouldCreate_WhenValueIsValid()
        {
            // arrange
            var email = _faker.Internet.Email();

            // act
            Action action = () => Email.Create(email);

            // assert
            action.ShouldNotThrow();
        }

        [Fact]
        public void CreateEmail_ShouldThrow_WhenEmailIsNullOrEmpty()
        {
            // arrange 
            var email = string.Empty;

            // act 
            Action action = () => Email.Create(email);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(EmailErrors.EmailRequired.Code);
            exception.Error.Message.ShouldBe(EmailErrors.EmailRequired.Message);
        }

        [Fact]
        public void CreateEmail_ShouldThrow_WhenFormatIsInvalid()
        {
            // arrange 
            var email = _faker.Random.String2(2);

            // act 
            Action action = () => Email.Create(email);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(EmailErrors.EmailFormatInvalid.Code);
            exception.Error.Message.ShouldBe(EmailErrors.EmailFormatInvalid.Message);
        }

        [Fact]
        public void CreateEmail_ShouldThrow_WhenEmailIsTooShort()
        {
            // arrange 
            var email = _faker.Random.String2(4);

            // act 
            Action action = () => Email.Create(email);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(EmailErrors.EmailTooShort.Code);
            exception.Error.Message.ShouldBe(EmailErrors.EmailTooShort.Message);
        }

        [Fact]
        public void CreateEmail_ShouldThrow_WhenEmailIsTooLong()
        {
            // arrange
            var email = _faker.Random.String2(256);

            // act 
            Action action = () => Email.Create(email);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(EmailErrors.EmailTooLong.Code);
            exception.Error.Message.ShouldBe(EmailErrors.EmailTooLong.Message);
        }
    }
}
