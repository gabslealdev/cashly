using Bogus;
using Cashly.Domain.IdentityContext.Errors;
using Cashly.Domain.IdentityContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Shouldly;

namespace Cashly.Domain.UnitTests.Identity.ValueObjects
{
    public class NameUnitTest
    {
        private readonly Faker _faker = new(); 

        [Fact]
        public void CreateName_ShouldCreate_WhenValuesIsValid()
        {
            // arrange
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();

            // act 
            Action action = () => Name.Create(firstName, lastName);

            // assert 
            action.ShouldNotThrow();
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenFirstNameIsNullOrEmpty()
        {
            // arrange
            var firstName = string.Empty;
            var lastName = _faker.Name.LastName();

            // act
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.FirstNameRequired.Code);
            exception.Error.Message.ShouldBe(NameErrors.FirstNameRequired.Message);
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenFirstNameIsTooShort()
        {
            // arrange
            var firstName = _faker.Random.String2(2);
            var lastName = _faker.Name.LastName();

            // act 
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.FirstNameTooShort.Code);
            exception.Error.Message.ShouldBe(NameErrors.FirstNameTooShort.Message);
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenFirstNameIsTooLong()
        {
            // arrange
            var firstName = _faker.Random.String2(80);
            var lastName = _faker.Name.LastName();

            // act 
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.FirstNameTooLong.Code);
            exception.Error.Message.ShouldBe(NameErrors.FirstNameTooLong.Message);
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenLastNameIsNullOrEmpty()
        {
            // arrange 
            var firstName = _faker.Name.FirstName();
            var lastName = string.Empty;

            // act 
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.LastNameRequired.Code);
            exception.Error.Message.ShouldBe(NameErrors.LastNameRequired.Message);
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenLastNameIsTooShort()
        {
            // arrange
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Random.String2(2);

            // act 
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.LastNameTooShort.Code);
            exception.Error.Message.ShouldBe(NameErrors.LastNameTooShort.Message);
        }

        [Fact]
        public void CreateName_ShouldThrow_WhenLastNameIsTooLong()
        {
            // arrange
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Random.String2(80);

            // act
            Action action = () => Name.Create(firstName, lastName);

            // assert
            var exception = action.ShouldThrow<DomainExceptionValidation>();
            exception.Error.Code.ShouldBe(NameErrors.LastNameTooLong.Code);
            exception.Error.Message.ShouldBe(NameErrors.LastNameTooLong.Message);
        }

        [Fact]
        public void ToString_ShouldReturnFullName_WhenValueIsValid()
        {
            // arrange 
            var firstName = "João";
            var lastName = "Da Silva";
            var name = Name.Create(firstName, lastName);

            // act
            var result = name.ToString();

            // assert
            result.ShouldBe("João Da Silva");
        }
    }
}
