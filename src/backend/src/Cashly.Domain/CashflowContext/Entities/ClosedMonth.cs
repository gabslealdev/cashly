using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Entities;
using Cashly.Domain.Shared.Exceptions;

namespace Cashly.Domain.CashflowContext.Entities;

public sealed class ClosedMonth : Entity
{
    public Guid CashflowId { get; private set; }
    public Period Period { get; private set; } = null!;
    public Money PeriodResult { get; private set; } = null!;
    public FinancialHealthStatus Status { get; private set; }
    public DateTimeOffset ClosedAt { get; private set; }
    
    private ClosedMonth(){}

    private ClosedMonth(
        Guid cashflowId,
        Period period,
        Money periodResult,
        FinancialHealthStatus status,
        DateTimeOffset closedAt)
    {
        CashflowId = cashflowId;
        Period = period;
        PeriodResult = periodResult;
        Status = status;
        ClosedAt = closedAt;
    }

    internal static ClosedMonth Create(
        Guid cashflowId,
        Period period,
        Money periodResult,
        FinancialHealthStatus status,
        DateTimeOffset closedAt)
    {
        Validate(cashflowId, period, periodResult, status);

        return new ClosedMonth(
            cashflowId,
            period,
            periodResult,
            status, 
            closedAt);
    }

    private static void Validate(
        Guid cashflowId,
        Period period,
        Money periodResult,
        FinancialHealthStatus status)
    {
        DomainExceptionValidation.When(cashflowId == Guid.Empty, ClosedMonthErrors.CashflowReferenceRequired);
        DomainExceptionValidation.When(period is null, ClosedMonthErrors.PeriodRequired);
        DomainExceptionValidation.When(periodResult is null, ClosedMonthErrors.PeriodResultRequired);
        DomainExceptionValidation.When(!Enum.IsDefined(status), ClosedMonthErrors.InvalidFinancialHealthStatus);
    }
}
