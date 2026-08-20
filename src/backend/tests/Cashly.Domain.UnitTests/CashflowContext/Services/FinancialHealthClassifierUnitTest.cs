using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.Services;

public sealed class FinancialHealthClassifierUnitTest
{
    private readonly FinancialHealthClassifier _classifier = new();

    public static TheoryData<decimal, decimal, decimal, FinancialHealthStatus> BoundaryScenarios => new()
    {
        { 1_000m,   699m,  0.301m, FinancialHealthStatus.Excellent },
        { 1_000m,   700m,  0.300m, FinancialHealthStatus.Excellent },
        { 1_000m,   701m,  0.299m, FinancialHealthStatus.Healthy },
        { 1_000m,   799m,  0.201m, FinancialHealthStatus.Healthy },
        { 1_000m,   800m,  0.200m, FinancialHealthStatus.Healthy },
        { 1_000m,   801m,  0.199m, FinancialHealthStatus.Attention },
        { 1_000m,   899m,  0.101m, FinancialHealthStatus.Attention },
        { 1_000m,   900m,  0.100m, FinancialHealthStatus.Attention },
        { 1_000m,   901m,  0.099m, FinancialHealthStatus.Warning },
        { 1_000m,   999m,  0.001m, FinancialHealthStatus.Warning },
        { 1_000m, 1_000m,  0.000m, FinancialHealthStatus.Critical },
        { 1_000m, 1_001m, -0.001m, FinancialHealthStatus.Critical }
    };

    [Fact]
    public void Classify_ShouldReturnNoActivity_WhenIncomeAndExpenseAreZero()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(0));

        // act
        var healthStatus = _classifier.Classify(financialResult);

        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.NoActivity);
    }

    [Fact]
    public void Classify_ShouldReturnCritical_WhenThereIsExpenseWithoutIncome()
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(0),
            Money.Create(1_000m));

        // act
        var healthStatus = _classifier.Classify(financialResult);

        // assert
        healthStatus.ShouldBe(FinancialHealthStatus.Critical);
    }

    [Theory]
    [MemberData(nameof(BoundaryScenarios))]
    public void Classify_ShouldReturnExpectedStatus_WhenResultPercentIsAtBoundary(
        decimal totalIncome,
        decimal totalExpense,
        decimal expectedResultPercent,
        FinancialHealthStatus expectedStatus)
    {
        // arrange
        var financialResult = PeriodFinancialResult.Create(
            Money.Create(totalIncome),
            Money.Create(totalExpense));

        // act
        var healthStatus = _classifier.Classify(financialResult);

        // assert
        financialResult.ResultPercent.ShouldBe(expectedResultPercent);
        healthStatus.ShouldBe(expectedStatus);
    }
}
