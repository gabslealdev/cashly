using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Entities;

namespace Cashly.Domain.CashflowContext.Entities;

public sealed class ClosedMonth : Entity
{
    public Guid CashflowId { get; private set; }
    public Period Period { get; private set; } = null!;
    public Money Balance { get; private set; } = null!;
    public FinancialHealthStatus Status { get; private set; }
    public DateTimeOffset ClosedAt { get; private set; }
    
    private ClosedMonth(){}

    private ClosedMonth(
        Guid cashflowId,
        Period period,
        Money balance,
        FinancialHealthStatus status,
        DateTimeOffset closedAt
        )
    {
        CashflowId = cashflowId;
        Period = period;
        Balance = balance;
        Status = status;
        ClosedAt = closedAt;
    }

    public static ClosedMonth Create(
        Guid cashflowId,
        Period period,
        Money balance,
        FinancialHealthStatus status,
        DateTimeOffset closedAt)
    {
        return new ClosedMonth(cashflowId, period, balance, status, closedAt);
    }
}