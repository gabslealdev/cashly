using System.Globalization;
using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.CashflowContext.ValueObjects;

public sealed record Money : ValueObject
{
    public decimal Value { get; private set; }

    private Money(){}

    private Money(decimal value)
    {
        Value = decimal.Round(value, 2);
    }
    
    public static Money Create(decimal value)
        => new Money(value);

    public override string ToString()
        => Value.ToString("F2", CultureInfo.InvariantCulture);
}