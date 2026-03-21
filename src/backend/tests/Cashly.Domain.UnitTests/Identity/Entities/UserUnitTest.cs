using Bogus;
using Cashly.Domain.Identity.Entities;
using Cashly.Domain.Identity.ValueObjects;
using Cashly.Domain.UnitTests.Identity.Builders;
using Shouldly;

namespace Cashly.Domain.UnitTests.Identity.Entities
{
    public class UserUnitTest
    {
        private readonly Faker _faker = new();

        [Fact]
        public void CreateUser_ShouldCreate_WhenUserIsValid()
        {
            // arrange 
            var name = Name.Create(firstName: _faker.Name.FirstName(), lastName: _faker.Name.LastName());
            var email = Email.Create(value: _faker.Internet.Email());
            var passswordHash = PasswordHash.Create(value: _faker.Random.Hash());

            // act
            Action action = () => User.Create(name, email, passswordHash);

            // assert
            action.ShouldNotThrow();
        }

        [Fact]
        public void UpdateName_ShouldUpdate_WhenNameIsValid()
        {
            // arrange 
            var name = Name.Create("João", "Silva");
            var user = new UserBuilder().WithName(name).Build();
            var updatedName = Name.Create("João", "Da Silva");

            // act
            user.UpdateName(updatedName);

            // assert
            user.Name.ShouldBe(updatedName);
   
        }

        [Fact]
        public void UpdateEmail_ShouldUpdate_WhenEmailIsValid()
        {
            // arrange 
            var email = Email.Create("joao_silva@cashly.com");
            var user = new UserBuilder().WithEmail(email).Build();
            var updatedEmail = Email.Create("dasilva.joao@cashlly.com");

            // act
            user.UpdateEmail(updatedEmail);

            // assert
            user.Email.ShouldBe(updatedEmail);
        }
    }
}
