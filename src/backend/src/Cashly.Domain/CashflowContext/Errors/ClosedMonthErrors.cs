using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CashflowContext.Errors;

public static class ClosedMonthErrors
{
    public static readonly DomainError CashflowReferenceRequired =
        new DomainError("ClosedMonth.Cashflow.Required", "Cashflow reference is required.");

    public static readonly DomainError PeriodRequired =
        new DomainError("ClosedMonth.Period.Required", "Period is required.");

    public static readonly DomainError PeriodResultRequired =
        new DomainError("ClosedMonth.PeriodResult.Required", "Period result is required.");

    public static readonly DomainError InvalidFinancialHealthStatus =
        new DomainError("ClosedMonth.FinancialHealthStatus.Invalid", "Financial health status is invalid.");
}
