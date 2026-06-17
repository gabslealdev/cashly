using Bogus;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Domain.UnitTests.CashflowContext.Builders;

public class ClosedMonthBuilder
{
    private readonly Faker _faker = new();
    private Guid? _cashflowId;
    private Period? _period;
    private Money? _balance;
    
    
}