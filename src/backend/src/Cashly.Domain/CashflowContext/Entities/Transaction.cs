using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Entities;

namespace Cashly.Domain.CashflowContext.Entities;

public sealed class Transaction : Entity
{
    public Guid CashflowId { get; private set; }
    public Title Title { get; private set; } = null!;
    public Amount Amount { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public TransactionStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

}