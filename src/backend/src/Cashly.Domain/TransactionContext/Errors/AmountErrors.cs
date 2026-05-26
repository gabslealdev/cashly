using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.TransactionContext.Errors;

public static class AmountErrors
{
    public static readonly DomainError AmountNegative = new DomainError("amount.isNegative", "Amount is negative");
}