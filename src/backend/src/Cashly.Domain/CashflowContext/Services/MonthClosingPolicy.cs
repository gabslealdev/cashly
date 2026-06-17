using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.Enums;

namespace Cashly.Domain.CashflowContext.Services;

public sealed class MonthClosingPolicy
{
    public void EnsureCanClose(Period period, IEnumerable<Transaction> transactions)
    {
        var hasScheduledTransactions = transactions.Any(transaction =>
            Period.From(transaction.Date) == period &&
            transaction.Status == TransactionStatus.Scheduled);

        DomainExceptionValidation.When(
            hasScheduledTransactions,
            CashflowErrors.ScheduledTransactionsCannotBeClosed);
    }
}
