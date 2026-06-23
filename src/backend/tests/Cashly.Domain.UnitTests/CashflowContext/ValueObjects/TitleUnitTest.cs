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
        var value = _faker.Finance.AccountName();

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

    [Fact]
    public void CreateTitle_ShouldNormalizeWithTrim_WhenValueHasOuterSpaces()
    {
        // arrange
        const string value = "  Main Cashflow  ";

        // act
        var title = Title.Create(value);

        // assert
        title.Value.ShouldBe("Main Cashflow");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(49)]
    public void CreateTitle_ShouldCreate_WhenLengthIsInsideBoundary(int length)
    {
        // arrange
        var value = new string('a', length);

        // act
        Action action = () => Title.Create(value);

        // assert
        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(3)]
    [InlineData(50)]
    public void CreateTitle_ShouldThrow_WhenLengthIsOutsideBoundary(int length)
    {
        // arrange
        var value = new string('a', length);

        // act
        Action action = () => Title.Create(value);

        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();

        if (length == 3)
        {
            exception.Error.Code.ShouldBe(TitleErrors.TitleTooShort.Code);
            exception.Error.Message.ShouldBe(TitleErrors.TitleTooShort.Message);
            return;
        }

        exception.Error.Code.ShouldBe(TitleErrors.TitleTooLong.Code);
        exception.Error.Message.ShouldBe(TitleErrors.TitleTooLong.Message);
    }
}
    
