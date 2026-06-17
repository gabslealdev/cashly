using Bogus;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Domain.UnitTests.CashflowContext.Builders;

public sealed class CashflowBuilder
{
    private readonly Faker _faker = new();
    private readonly List<Period> _closedPeriods = [];
    private Title?  _title;
    private Guid? _userId;

    public CashflowBuilder WithTitle(Title title)
    {
        _title =  title;
        return this;
    }

    public CashflowBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public CashflowBuilder WithClosedMonth(Period period)
    {
        _closedPeriods.Add(period);
        return this;
    }
    
    public Cashflow Build()
    {
        var title = _title ?? Title.Create(_faker.Finance.AccountName());
        var userId = _userId ?? Guid.NewGuid();
        
        var cashflow = Cashflow.Create(title, userId);

        foreach (var period in _closedPeriods)
        {
            cashflow.CloseMonth(
                period,
                PeriodFinancialResult.Create(
                    Money.Create(1000.00m),
                    Money.Create(500.00m)),
                FinancialHealthStatus.Healthy,
                DateTimeOffset.UtcNow);
        }
        
        return cashflow;
    }
}