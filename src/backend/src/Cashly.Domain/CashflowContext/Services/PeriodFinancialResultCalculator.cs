using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.Enums;

namespace Cashly.Domain.CashflowContext.Services;

public sealed class PeriodFinancialResultCalculator
{
    public PeriodFinancialResult Calculate(Period period, IEnumerable<Transaction> transactions)
    {
        decimal totalIncome = 0;
        decimal totalExpense = 0;

        foreach (var transaction in transactions)
        {
            if (Period.From(transaction.Date) != period)
                continue;
            
            if (transaction.Status != TransactionStatus.Completed)
                continue;
            
            if (transaction.Type == TransactionType.Income)
                totalIncome += transaction.Amount.Value;
            
            if (transaction.Type == TransactionType.Expense)
                totalExpense += transaction.Amount.Value;
        }
        
        
        return PeriodFinancialResult.Create(
            Money.Create(totalIncome), 
            Money.Create(totalExpense));
    }
}
