using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.Services;

public sealed class FinancialHealthClassifierUnitTest
{
    [Fact]
    public void Classify_ShouldReturnNoActivity_WhenIncomeAndExpenseAreZero()
    {
        // arrange 
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(0));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.NoActivity);
    }

    [Fact]
    public void Classify_ShouldReturnCritical_WhenThereIsExpenseWithoutIncome()
    {
        // arrange 
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(1000));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);

        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Critical);
    }

    [Fact]
    public void Classify_ShouldReturnExcellent_WhenResultPercentIsEqualToThirtyPercent()
    {
        // arrange 
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(300));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Excellent);
    }

    [Fact]
    public void Classify_ShouldReturnExcellent_WhenResultPercentIsGreaterThanThirtyPercent()
    {
        // arrange 
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(100));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Excellent);
    }

    [Fact]
    public void Classify_ShouldReturnHealth_WhenResultPercentIsEqualToTwentyPercent()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(200));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);

        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Healthy);
    }
    
    [Fact]
    public void Classify_ShouldReturnHealth_WhenResultPercentIsGreaterThanTwentyPercent()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(260));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);

        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Healthy);
    }

    [Fact]
    public void Classify_ShouldReturnAttention_WhenResultPercentIsEqualToTenPercent()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(100));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Attention);
    }
    
    [Fact]
    public void Classify_ShouldReturnAttention_WhenResultPercentIsGreaterThanTenPercent()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(130));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Attention);
    }
    
    [Fact]
    public void Classify_ShouldReturnWarning_WhenResultPercentIsGreaterThanZeroPercent()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(1000),
            Money.Create(17));
        
        // act
        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);
        
        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Warning);
    }
}
