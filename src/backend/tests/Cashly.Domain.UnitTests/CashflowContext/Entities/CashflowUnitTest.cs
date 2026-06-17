using Bogus;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.UnitTests.CashflowContext.Builders;
using Cashly.Domain.UnitTests.IdentityContext.Builders;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.Entities;

public class CashflowUnitTest
{
    private readonly Faker _faker = new();

    [Fact]
    public void CreateCashflow_ShouldCreate_WhenIsValid()
    {
        // arrange
        var user = new UserBuilder().Build();
        var title = Title.Create(value: _faker.Finance.AccountName());
        
        // act 
        Action action = () => Cashflow.Create(title, user.Id);
        
        // assert
        action.ShouldNotThrow();
    }
    
    [Fact]
    public void CreateCashflow_ShouldThrow_WhenOwnerIsInvalid()
    {
        // arrange
        var userid = Guid.Empty;
        var title = Title.Create(_faker.Finance.AccountName());
        
        // act
        Action action = () => Cashflow.Create(title, userid);
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(CashflowErrors.InvalidOwner.Code);
        exception.Error.Message.ShouldBe(CashflowErrors.InvalidOwner.Message);
    }

    [Fact]
    public void EnsureTransactionRegistration_ShouldNotThrow_WhenRegistrationDataIsValid()
    {
        // arrange
        var userId = _faker.Random.Guid();
        var cashflow = new CashflowBuilder().WithUserId(userId).Build();
        var date =  DateTimeOffset.UtcNow;

        // act 
        Action action = () => cashflow.EnsureTransactionRegistration(userId, date);
        
        // assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void EnsureTransactionRegistration_ShouldThrow_WhenTransactionDateIsInvalid()
    {
        // arrange
        var date =  DateTimeOffset.UtcNow.AddMonths(-1);
        var userId = _faker.Random.Guid();
        var period = Period.From(date);
        
        var cashflow = new CashflowBuilder()
            .WithUserId(userId)
            .WithClosedMonth(period)
            .Build();
        
        // act
        Action action = () => cashflow.EnsureTransactionRegistration(userId, date);
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(CashflowErrors.MonthIsClosed.Code);
        exception.Error.Message.ShouldBe(CashflowErrors.MonthIsClosed.Message);
    }

    [Fact]
    public void EnsureTransactionRegistration_ShouldThrow_WhenUserIsInvalid()
    {   
        // arrange
        var userId = _faker.Random.Guid();
        var date = DateTimeOffset.UtcNow;
        var cashflow = new CashflowBuilder().Build();
        
        // act
        Action action = () => cashflow.EnsureTransactionRegistration(userId,  date);
        
        // assert
        var exception =  action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(CashflowErrors.PermissionDenied.Code);
        exception.Error.Message.ShouldBe(CashflowErrors.PermissionDenied.Message);
        
    }

    [Fact]
    public void CloseMonth_ShouldClose_WhenPeriodIsValid()
    {
        // arrange
        var period = Period.From(DateTime.UtcNow.AddMonths(-1));
        var periodFinancialResult = PeriodFinancialResult.Create(Money.Create(2000.00m), Money.Create(500.00m));
        var closedAt = DateTimeOffset.UtcNow;
        var cashflow = new CashflowBuilder().Build();
        
        // act
        Action action = () => cashflow.CloseMonth(
            period,
            periodFinancialResult,
            FinancialHealthStatus.Excellent,
            closedAt);
        
        // assert
        action.ShouldNotThrow();
        
    }

    [Fact]
    public void CloseMonth_ShouldThrow_WhenMonthIsAlreadyClosed()
    {
        // arrange
        var period = Period.From(DateTime.UtcNow.AddMonths(-1));
        var cashflow = new CashflowBuilder().WithClosedMonth(period).Build();
        var periodFinancialResult = PeriodFinancialResult.Create(Money.Create(2000.00m), Money.Create(500.00m));
        var closedAt = DateTimeOffset.UtcNow;
        
        // act 
        Action action = () => cashflow.CloseMonth(
            period,
            periodFinancialResult,
            FinancialHealthStatus.Excellent,
            closedAt);
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Error.Code.ShouldBe(CashflowErrors.MonthIsClosed.Code);
        exception.Error.Message.ShouldBe(CashflowErrors.MonthIsClosed.Message);
    }
}