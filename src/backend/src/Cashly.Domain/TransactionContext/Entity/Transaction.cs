using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.TransactionContext.ValueObjects;

namespace Cashly.Domain.TransactionContext.Entity;

public sealed class Transaction : Shared.Entities.Entity
{
    public Guid CashflowId { get; private set; }
    public Title Title { get; private set; } = null!;
    public Amount Amount { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public TransactionStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Transaction(){}

    private Transaction(Guid cashflowId, Title title, Amount amount, TransactionType type, 
        DateTimeOffset date, TransactionStatus status)
    {
        CashflowId = cashflowId;
        Title = title;
        Amount = amount;
        Type = type;
        Date = date;
        Status = status;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public static Transaction Create(Guid cashflowId, Title title, Amount amount, TransactionType type, 
        DateTimeOffset date, TransactionStatus status) 
        => new Transaction(cashflowId, title, amount, type, date, status);
}
