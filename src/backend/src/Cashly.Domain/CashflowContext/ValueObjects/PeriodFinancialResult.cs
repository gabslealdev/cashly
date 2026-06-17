using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.CashflowContext.ValueObjects;

public sealed record PeriodFinancialResult : ValueObject
{
    public Money TotalIncome { get; private set; }
    public Money TotalExpense { get; private set; }
    public Money PeriodResult { get; private set; } 
    public decimal ResultPercent => TotalIncome.Value == 0 ? 0 : PeriodResult.Value / TotalIncome.Value;
    
    private PeriodFinancialResult(){}

    private PeriodFinancialResult(Money totalIncome, Money totalExpense)
    {
        TotalIncome = totalIncome;
        TotalExpense = totalExpense;
        PeriodResult = Money.Create(totalIncome.Value - totalExpense.Value);
    }
    
    public static PeriodFinancialResult Create(Money totalIncome, Money totalExpense)
        => new(totalIncome, totalExpense);
}
