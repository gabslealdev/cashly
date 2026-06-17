using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.CollaborationContext.Entities;
using Cashly.Domain.CollaborationContext.Enums;
using Cashly.Domain.Shared.Entities;
using Cashly.Domain.Shared.Exceptions;

namespace Cashly.Domain.CashflowContext.Entities;

public sealed class Cashflow : Entity
{
    private readonly List<CashflowMember> _cashflowMembers = [];
    private readonly List<ClosedMonth> _closedMonths = [];
    
    public Title Title { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    
    public IReadOnlyList<CashflowMember> CashflowMembers => _cashflowMembers;
    public IReadOnlyList<ClosedMonth> ClosedMonths => _closedMonths;


    private Cashflow() {}

    private Cashflow(Title title)
    {
        Title = title;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    

    public static Cashflow Create(Title title, Guid userId)
    {
       var cashflow = new Cashflow(title); 
       cashflow.AssignOwner(userId);

       return cashflow;
    }

    public void EnsureTransactionRegistration(Guid userId, DateTimeOffset transactionDate)
    {
        DomainExceptionValidation.When(IsClosedMonth(Period.From(transactionDate)), CashflowErrors.MonthIsClosed);
        DomainExceptionValidation.When(!CanRegisterTransaction(userId), CashflowErrors.PermissionDenied);
    }

    public ClosedMonth CloseMonth(
        Period period,
        PeriodFinancialResult financialResult,
        FinancialHealthStatus status,
        DateTimeOffset closedAt)
    {
        DomainExceptionValidation.When(IsClosedMonth(period), CashflowErrors.MonthIsClosed);

        var closedMonth = ClosedMonth.Create(
            cashflowId: Id,
            period: period,
            periodResult: financialResult.PeriodResult,
            status: status,
            closedAt: closedAt
        );

        _closedMonths.Add(closedMonth);
        UpdatedAt = DateTimeOffset.UtcNow;

        return closedMonth;
    }
    
    private void AssignOwner(Guid userId)
    {
        DomainExceptionValidation.When(userId == Guid.Empty, CashflowErrors.InvalidOwner);

        DomainExceptionValidation.When(
            _cashflowMembers.Any(x => x.Role == CashflowMemberRole.Owner),
            CashflowErrors.OwnerAlreadyExists);

        var owner = CashflowMember.CreateOwner(Id, userId);
        _cashflowMembers.Add(owner);

        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private bool CanRegisterTransaction(Guid userId)
        => IsOwner(userId) || IsContributor(userId);
    
    private bool IsOwner(Guid userId)
        => HasRole(userId, CashflowMemberRole.Owner);
    
    private bool IsContributor(Guid userId) 
        => HasRole(userId, CashflowMemberRole.Contributor);

    private bool HasRole(Guid userId, CashflowMemberRole role)
    {
        var member = GetMember(userId);
        return member is not null && (member.Role == role);
    }
    
    private bool IsClosedMonth(Period period)
     => _closedMonths.Any(x => x.Period == period);
    
    private CashflowMember? GetMember(Guid userId)
        =>  _cashflowMembers.FirstOrDefault(x => x.UserId == userId);
}
