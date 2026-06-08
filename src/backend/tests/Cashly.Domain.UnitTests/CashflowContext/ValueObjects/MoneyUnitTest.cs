using Bogus;
using Cashly.Domain.CashflowContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.ValueObjects;

public class MoneyUnitTest
{
    private readonly Faker _faker = new();

    public void CreateMoney_ShouldCreate_WhenMoneyIsValid()
    {
        // arrange 
        var value = _faker.Finance.Amount();
        
        // act
        Action action = () => Money.Create(value);

        // assert
        action.ShouldNotThrow();
    }
    
}