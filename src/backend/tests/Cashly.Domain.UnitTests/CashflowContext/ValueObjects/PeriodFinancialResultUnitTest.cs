using Bogus;
using Cashly.Domain.CashflowContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.ValueObjects;

public class PeriodFinancialResultUnitTest
{
    private readonly Faker _faker = new();
    
    [Fact]
    public void CreatePeriodFinancialResult_ShouldCreate_WhenIsValid()
    {
        // arrange 
        var totalIncome = Money.Create(_faker.Random.Decimal(2000.00m, 7000.00m));
        var totalExpense = Money.Create(_faker.Random.Decimal(2000.00m, 7000.00m));
        
        // act 
        Action action = () => PeriodFinancialResult.Create(totalIncome, totalExpense);
        
        // assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void CreatePeriodFinancialResult_ShouldBeZero_WhenIncomeIsZero()
    {
        // arrange
        var totalIncome = 0;
        var totalExpense = 0;
        
        // act
        var result = PeriodFinancialResult.Create(Money.Create(totalIncome), Money.Create(totalExpense));
        
        // assert
        result.ResultPercent.ShouldBe(0);
    }

    [Theory]
    [InlineData(100, 80, 0.20)]
    [InlineData(100, 70, 0.40)]
    [InlineData(100, 70,  0.60)]
    [InlineData(200, 150,  0.25)]
    public void ResultPercent_ShouldReturnPeriodFinancialResultDivideByTotalIncome(decimal totalIncome, decimal totalExpense, decimal expectedPercent)
    {
        // act 
        var result = PeriodFinancialResult.Create(Money.Create(totalIncome), Money.Create(totalExpense));
        
        // assert
        result.ResultPercent.ShouldBe(expectedPercent);
    }
}