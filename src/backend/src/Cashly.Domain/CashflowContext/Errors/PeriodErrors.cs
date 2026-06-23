using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CashflowContext.Errors;

public static class PeriodErrors
{
    public static readonly DomainError InvalidMonth = new DomainError("Invalid.Month", "Month is invalid");
    public static readonly DomainError InvalidYear = new DomainError("Invalid.Year", "Year is invalid");
}