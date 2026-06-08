using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.CashflowContext.ValueObjects;

public sealed record Period : ValueObject, IComparable<Period>
{
    private const int MonthMinValue = 1;
    private const int MonthMaxValue = 12;
    private const int YearMinValue = 1900;
    public int Year { get; private set; }
    public int Month { get; private set; }

    private Period(){}

    private Period(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static Period Create(int year, int month)
    {
        Validate(year, month);
        return new Period(year, month);
    }
    
    private static void Validate(int year, int month)
    {
        DomainExceptionValidation.When(month is < MonthMinValue or > MonthMaxValue, PeriodErrors.InvalidMonth);
        DomainExceptionValidation.When(year < YearMinValue, PeriodErrors.InvalidYear);
    }
    
    private int SortValue => Year * 100 + Month;
    
    public int CompareTo(Period? other)
        => other is null ? 1 : SortValue.CompareTo(other.SortValue);
    
    public static bool operator <(Period left, Period right)
        => left.CompareTo(right) < 0;
    
    public static bool operator <=(Period left, Period right)
        => left.CompareTo(right) <= 0;

    public static bool operator >(Period left, Period right)
        => left.CompareTo(right) > 0;
    
    public static bool operator >=(Period left, Period right)
        => left.CompareTo(right) >= 0;

    public override string ToString() 
        => $"{Month:D2}/{Year:D4}";

    public static Period From(DateTimeOffset date)
        => Create(date.Year, date.Month);
}