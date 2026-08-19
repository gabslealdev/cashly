
using Bogus;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.TransactionContext.ValueObjects;
using TransactionStatus = Cashly.Domain.TransactionContext.Enums.TransactionStatus;

namespace Cashly.Domain.UnitTests.TransactionContext.Builders;

public sealed class TransactionBuilder
{
    private readonly Faker _faker = new();
    private Guid? _cashflowId;
    private Title? _title;
    private Amount? _amount;
    private TransactionType? _type;
    private DateTimeOffset? _date;
    private TransactionStatus? _status;

    public TransactionBuilder WithCashflowId(Guid cashflowId)
    {
        _cashflowId = cashflowId;
        return this;
    }

    public TransactionBuilder WithTitle(Title title)
    {
        _title = title;
        return this;
    }

    public TransactionBuilder WithAmount(Amount amount)
    {
        _amount = amount;
        return this;
    }

    public TransactionBuilder WithType(TransactionType type)
    {
        _type =  type;
        return this;
    }

    public TransactionBuilder WithDate(DateTimeOffset date)
    {
        _date = date;
        return this;
    }

    public TransactionBuilder WithStatus(TransactionStatus status)
    {
        _status = status;
        return this;
    }

    public Transaction Build()
    {
        var cashflowId = _faker.Random.Guid();
        var title = _title ?? Title.Create(_faker.Commerce.ProductName());
        var amount = _amount ?? Amount.Create(_faker.Commerce.Random.Decimal());
        var type = _type ?? TransactionType.Expense;
        var date = _date ?? _faker.Date.RecentOffset();
        var status = _status ?? TransactionStatus.Completed;
        
        return Transaction.Create(cashflowId, title, amount, type, date, status);
    }
}