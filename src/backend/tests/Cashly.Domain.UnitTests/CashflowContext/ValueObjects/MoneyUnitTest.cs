using Bogus;
using Cashly.Domain.CashflowContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.ValueObjects;

public class MoneyUnitTest
{
    private readonly Faker _faker = new();
    
    [Fact]
    public void CreateMoney_ShouldCreate_WhenMoneyIsValid()
    {
        // arrange 
        var value = _faker.Finance.Amount();
        
        // act
        Action action = () => Money.Create(value);

        // assert
        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(10.555, 10.56)]
    [InlineData(10.554, 10.55)]
    [InlineData(10, 10)]
    public void CreateMoney_ShouldRoundValueToTwoDecimalPlaces(decimal value, decimal expectedValue)
    {
        // act
        var money = Money.Create(value);

        // assert
        money.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void CreateMoney_ShouldCreate_WhenValueIsNegative()
    {
        // arrange
        const decimal value = -10.50m;

        // act
        var money = Money.Create(value);

        // assert
        money.Value.ShouldBe(value);
    }

    [Fact]
    public void ToString_ShouldFormatValueWithTwoDecimalPlaces()
    {
        // arrange
        var money = Money.Create(10.5m);

        // act
        var result = money.ToString();

        // assert
        result.ShouldBe("10.50");
    }
    
}
