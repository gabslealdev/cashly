using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.Shared.Exceptions;

namespace Cashly.Domain.CashflowContext.ValueObjects;

public sealed record Title
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    public string Value { get; private set; } = string.Empty;
    
    private Title(){}

    private Title(string value)
    {
      Value = value;
    }

    public static Title Create(string value)
    {
        var normalizedValue = Normalize(value);
        Validate(normalizedValue);
        
        return new Title(normalizedValue);
    }
    
    private static string Normalize(string value)
        => value.Trim().ToLowerInvariant();

    private static void Validate(string value)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(value), TitleErrors.TitleRequired);
        DomainExceptionValidation.When(value.Length <= MinLength, TitleErrors.TitleTooShort);
        DomainExceptionValidation.When(value.Length >= MaxLength, TitleErrors.TitleTooLong);
    }
}