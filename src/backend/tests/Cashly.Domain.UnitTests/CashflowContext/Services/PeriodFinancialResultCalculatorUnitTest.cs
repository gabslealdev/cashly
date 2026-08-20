using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.UnitTests.TransactionContext.Builders;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.Services;

public sealed class PeriodFinancialResultCalculatorUnitTest
{
    private readonly PeriodFinancialResultCalculator _calculator = new();
    private static readonly DateTimeOffset PeriodDate =
        new(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Calculate_ShouldCalculateFinancialResult_WhenTransactionsAreCompleted()
    {
        // arrange
        var period = Period.From(PeriodDate);
        var transactions = new[]
        {
            CreateTransaction(2_000m, TransactionType.Income),
            CreateTransaction(500m, TransactionType.Expense),
            CreateTransaction(250m, TransactionType.Expense)
        };

        // act
        var result = _calculator.Calculate(period, transactions);

        // assert
        result.TotalIncome.Value.ShouldBe(2_000m);
        result.TotalExpense.Value.ShouldBe(750m);
        result.PeriodResult.Value.ShouldBe(1_250m);
    }

    [Theory]
    [InlineData(TransactionStatus.Scheduled)]
    [InlineData(TransactionStatus.Canceled)]
    public void Calculate_ShouldIgnoreTransaction_WhenStatusIsNotCompleted(TransactionStatus status)
    {
        // arrange
        var period = Period.From(PeriodDate);
        var transaction = new TransactionBuilder()
            .WithAmount(Cashly.Domain.TransactionContext.ValueObjects.Amount.Create(1_000m))
            .WithType(TransactionType.Income)
            .WithDate(PeriodDate)
            .WithStatus(status)
            .Build();

        // act
        var result = _calculator.Calculate(period, [transaction]);

        // assert
        result.TotalIncome.Value.ShouldBe(0m);
        result.TotalExpense.Value.ShouldBe(0m);
        result.PeriodResult.Value.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_ShouldIgnoreTransaction_WhenTransactionIsFromAnotherPeriod()
    {
        // arrange
        var period = Period.From(PeriodDate);
        var transaction = new TransactionBuilder()
            .WithAmount(Cashly.Domain.TransactionContext.ValueObjects.Amount.Create(1_000m))
            .WithType(TransactionType.Income)
            .WithDate(PeriodDate.AddMonths(1))
            .WithStatus(TransactionStatus.Completed)
            .Build();

        // act
        var result = _calculator.Calculate(period, [transaction]);

        // assert
        result.TotalIncome.Value.ShouldBe(0m);
        result.TotalExpense.Value.ShouldBe(0m);
        result.PeriodResult.Value.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_ShouldReturnZeroResult_WhenTransactionsAreEmpty()
    {
        // arrange
        var period = Period.From(PeriodDate);

        // act
        var result = _calculator.Calculate(period, []);

        // assert
        result.TotalIncome.Value.ShouldBe(0m);
        result.TotalExpense.Value.ShouldBe(0m);
        result.PeriodResult.Value.ShouldBe(0m);
    }

    private static Cashly.Domain.TransactionContext.Entity.Transaction CreateTransaction(
        decimal amount,
        TransactionType type)
    {
        return new TransactionBuilder()
            .WithAmount(Cashly.Domain.TransactionContext.ValueObjects.Amount.Create(amount))
            .WithType(type)
            .WithDate(PeriodDate)
            .WithStatus(TransactionStatus.Completed)
            .Build();
    }
}
