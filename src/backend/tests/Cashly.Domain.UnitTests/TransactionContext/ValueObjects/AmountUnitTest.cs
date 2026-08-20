using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.TransactionContext.Errors;
using Cashly.Domain.TransactionContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.TransactionContext.ValueObjects;

public sealed class AmountUnitTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(100.50)]
    public void Create_ShouldCreateAmount_WhenValueIsNotNegative(decimal value)
    {
        // arrange

        // act
        var amount = Amount.Create(value);

        // assert
        amount.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_ShouldThrow_WhenValueIsNegative()
    {
        // arrange
        const decimal value = -0.01m;

        // act
        Action action = () => Amount.Create(value);

        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(AmountErrors.AmountNegative.Code);
        exception.Error.Message.ShouldBe(AmountErrors.AmountNegative.Message);
    }

    [Theory]
    [InlineData(10.555, 10.56)]
    [InlineData(10.554, 10.55)]
    [InlineData(10, 10)]
    public void Create_ShouldRoundValueToTwoDecimalPlaces(decimal value, decimal expectedValue)
    {
        // arrange

        // act
        var amount = Amount.Create(value);

        // assert
        amount.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void Addition_ShouldReturnSumOfAmounts()
    {
        // arrange
        var left = Amount.Create(10.25m);
        var right = Amount.Create(20.50m);

        // act
        var result = left + right;

        // assert
        result.Value.ShouldBe(30.75m);
    }

    [Fact]
    public void ToString_ShouldReturnValueWithTwoDecimalPlaces()
    {
        // arrange
        var amount = Amount.Create(10.5m);

        // act
        var result = amount.ToString();

        // assert
        result.ShouldBe("10.50");
    }
}
