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
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(0));

        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);

        healthStatus.ShouldBe(FinancialHealthStatus.NoActivity);
    }

    [Fact]
    public void Classify_ShouldReturnCritical_WhenThereIsExpenseWithoutIncome()
    {
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(100));

        var financialHealth = new FinancialHealthClassifier();
        var healthStatus = financialHealth.Classify(financialResult);

        healthStatus.ShouldBe(FinancialHealthStatus.Critical);
    }
}
