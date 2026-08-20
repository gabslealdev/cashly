using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.TransactionContext.ValueObjects;
using Cashly.Domain.UnitTests.TransactionContext.Builders;
using Shouldly;

namespace Cashly.Domain.UnitTests.TransactionContext.Entities;

public sealed class TransactionUnitTest
{
    [Fact]
    public void Create_ShouldCreateTransaction_WhenDataIsValid()
    {
        // arrange
        var cashflowId = Guid.NewGuid();
        var title = Title.Create("Monthly salary");
        var amount = Amount.Create(5_000m);
        var type = TransactionType.Income;
        var date = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        var status = TransactionStatus.Completed;
        var beforeCreation = DateTimeOffset.UtcNow;

        // act
        var transaction = new TransactionBuilder()
            .WithCashflowId(cashflowId)
            .WithTitle(title)
            .WithAmount(amount)
            .WithType(type)
            .WithDate(date)
            .WithStatus(status)
            .Build();

        var afterCreation = DateTimeOffset.UtcNow;

        // assert
        transaction.Id.ShouldNotBe(Guid.Empty);
        transaction.CashflowId.ShouldBe(cashflowId);
        transaction.Title.ShouldBe(title);
        transaction.Amount.ShouldBe(amount);
        transaction.Type.ShouldBe(type);
        transaction.Date.ShouldBe(date);
        transaction.Status.ShouldBe(status);
        transaction.CreatedAt.ShouldBeInRange(beforeCreation, afterCreation);
        transaction.UpdatedAt.ShouldBeInRange(beforeCreation, afterCreation);
    }

    [Theory]
    [InlineData(TransactionType.Income)]
    [InlineData(TransactionType.Expense)]
    public void Create_ShouldCreateTransaction_WithExpectedType(TransactionType type)
    {
        // arrange
        var builder = new TransactionBuilder().WithType(type);

        // act
        var transaction = builder.Build();

        // assert
        transaction.Type.ShouldBe(type);
    }

    [Theory]
    [InlineData(TransactionStatus.Scheduled)]
    [InlineData(TransactionStatus.Completed)]
    [InlineData(TransactionStatus.Canceled)]
    public void Create_ShouldCreateTransaction_WithExpectedStatus(TransactionStatus status)
    {
        // arrange
        var builder = new TransactionBuilder().WithStatus(status);

        // act
        var transaction = builder.Build();

        // assert
        transaction.Status.ShouldBe(status);
    }
}
