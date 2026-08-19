using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.UnitTests.TransactionContext.Builders;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.Services;

public sealed class MonthClosingPolicyUnitTest
{
    private readonly MonthClosingPolicy _policy = new();
    
    [Fact]
    public void EnsureCanClose_ShouldNotThrow_WhenTransactionsAreEmpty()
    {
        // arrange 
        var period = Period.Create(2025, 11);
        
        // act 
        Action action = () => _policy.EnsureCanClose(period, []);
        
        // assert
        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(TransactionStatus.Canceled)]
    [InlineData(TransactionStatus.Completed)]
    public void EnsureCanClose_(TransactionStatus status)
    {
        // arrange 
        var period = Period.Create(2025, 11);
        var transaction = new TransactionBuilder().WithStatus(status).Build();
        
        // act 
        Action action = () => _policy.EnsureCanClose(period, new[] { transaction });
        
        // assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void EnsureCanClose_ShouldThrow_WhenTransactionAreScheduled()
    {
        // arrange
        var period = Period.Create(2025, 11);
        var transaction = new TransactionBuilder().WithStatus(TransactionStatus.Scheduled).Build();
        
        // act
        Action action = () => _policy.EnsureCanClose(period, new[] { transaction });
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(CashflowErrors.ScheduledTransactionsCannotBeClosed.Code);
        exception.Error.Message.ShouldBe(CashflowErrors.ScheduledTransactionsCannotBeClosed.Message);
    }
}