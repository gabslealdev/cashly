using Bogus;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Shouldly;

namespace Cashly.Domain.UnitTests.CashflowContext.ValueObjects;

public class PeriodUnitTest
{
    private readonly Faker _faker = new();
    [Fact]
    public void CreatePeriod_ShouldCreate_WhenPeriodIsValid()
    {
        // arrange 
        var month = _faker.Random.Int(1, 12);
        var year = _faker.Random.Int(1900, 9999);
        
        // act
        Action action = () =>  Period.Create(month, year);
        
        // assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void CreatePeriod_ShouldThrow_WhenMonthIsInvalid()
    {
        // arrange 
        var month =  _faker.Random.Int(13, 99);
        var  year = _faker.Random.Int(1900, 9999);
        
        // act
        Action action  = () =>  Period.Create(month, year);
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Code.ShouldBe(PeriodErrors.InvalidMonth.Code);
        exception.Message.ShouldBe(PeriodErrors.InvalidMonth.Message);
    }
    
    [Fact]
    public void CreatePeriod_ShouldThrow_WhenYearIsInvalid()
    {
        // arrange 
        var month = _faker.Random.Int(1, 12);
        var year = _faker.Random.Int(1, 1900);
        
        // act 
        Action action = () =>  Period.Create(month, year);
        
        // assert
        var exception = action.ShouldThrow<DomainExceptionValidation>();
        exception.Code.ShouldBe(PeriodErrors.InvalidYear.Code);
        exception.Message.ShouldBe(PeriodErrors.InvalidYear.Message);
        
    }
    
    [Fact]
    public void CompareTo_ShouldReturnZero_WhenPeriodsAreEqual()
    {
        // arrange
        var date = _faker.Date.BetweenOffset(DateTimeOffset.Now, DateTimeOffset.Now.AddMonths(-1));
        var period = Period.From(date);
        var samePeriod = Period.From(date);
        
        // act 
        var result = period.CompareTo(samePeriod);
        
        // assert
        result.ShouldBe(0);
        
    }
    
    [Fact]
    public void CompareTo_ShouldBeTrue_WhenPeriodIsAfterOtherPeriod()
    {
        // arrange
        var date = _faker.Date.BetweenOffset(DateTimeOffset.Now, DateTimeOffset.Now.AddMonths(1));
        var period = Period.From(date);

        var pastDate = _faker.Date.BetweenOffset(DateTimeOffset.Now.AddMonths(-1), DateTimeOffset.Now);
        var pastPeriod = Period.From(pastDate);
        
        // act
        var result = period > pastPeriod;
        
        // assert
        result.ShouldBe(true);
    }
    
    [Fact]
    public void CompareTO_ShouldBeTrue_WhenPeriodIsBeforeOtherPeriod()
    {
        // arrange 
        var date = _faker.Date.BetweenOffset(DateTimeOffset.Now, DateTimeOffset.Now.AddMonths(1));
        var period =  Period.From(date);
        
        var futureDate =  _faker.Date.BetweenOffset(DateTimeOffset.Now.AddMonths(1), DateTimeOffset.Now.AddMonths(2));
        var futurePeriod = Period.From(futureDate);
        // act 
        var result = period < futurePeriod;
        
        // assert
        result.ShouldBe(true);
    }
    
    [Fact]
    public void ToString_ShouldBeTrue_When()
    {
        // arrange
        var date = new DateTimeOffset(2025, 12, 01, 00, 00, 00, TimeSpan.Zero);
        var  period = Period.From(date);
        // act 
        var result = period.ToString() == "12/2025";
        
        // assert
        result.ShouldBe(true);
    }
}
