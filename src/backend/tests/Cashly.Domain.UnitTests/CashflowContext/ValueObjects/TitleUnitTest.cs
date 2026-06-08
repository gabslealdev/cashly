using Bogus;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.ValueObjects;

public class TitleUnitTest
{
    private readonly Faker _faker = new();

    [Fact]
    public void CreateTitle_ShouldCreate_WhenValueIsValid()
    {
        // arrange
        var value = _faker.Finance.Account();

        //act
        Action action = () => Title.Create(value);

        // assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void CreateTitle_ShouldThrow_WhenTitleIsNullOrEmpty()
    {
        // arrange
        var value = "   ";

        //act
        Action action = () => Title.Create(value);

        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(TitleErrors.TitleRequired.Code);
        exception.Error.Message.ShouldBe(TitleErrors.TitleRequired.Message);
    }

    [Fact]
    public void CreateTitle_ShouldThrow_WhenTitleIsTooLong()
    {
        // arrange
        var value = _faker.Random.String(51);

        //act
        Action action = () => Title.Create(value);

        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(TitleErrors.TitleTooLong.Code);
        exception.Error.Message.ShouldBe(TitleErrors.TitleTooLong.Message);
    }
    [Fact]
    public void CreateTitle_ShouldThrow_WhenTitleIsTooShort()
    {
        // arrange
        var value = _faker.Random.String(1);

        //act
        Action action = () => Title.Create(value);

        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(TitleErrors.TitleTooShort.Code);
        exception.Error.Message.ShouldBe(TitleErrors.TitleTooShort.Message);
    }
}
    
