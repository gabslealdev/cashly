using System.Globalization;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.CashflowContext.ValueObjects;

public sealed record Amount : ValueObject
{
    public decimal Value { get; private set; }

    private Amount(){}

    private Amount(decimal value)
    {
        Value = decimal.Round(value, 2);
    }

    private static void Validate(decimal value)
    {
        DomainExceptionValidation.When(value < 0, AmountErrors.AmountNegative);
    }

    public static Amount Create(decimal value)
    {
        Validate(value);
        return new Amount(value);
    }
    
    public static Amount operator +(Amount left, Amount right)
        => Create(left.Value + right.Value);
    
    public override string ToString()
        => Value.ToString("F2", CultureInfo.InvariantCulture);
}