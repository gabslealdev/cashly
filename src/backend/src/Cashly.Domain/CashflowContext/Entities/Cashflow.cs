using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.CollaborationContext.Entities;
using Cashly.Domain.CollaborationContext.Enums;
using Cashly.Domain.Shared.Entities;
using Cashly.Domain.Shared.Exceptions;

namespace Cashly.Domain.CashflowContext.Entities;

public sealed class Cashflow : Entity
{
    private readonly List<CashflowMember> _cashflowMembers = [];
    
    public Title Title { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    
    public IReadOnlyList<CashflowMember> CashflowMembers => _cashflowMembers;

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
    
    
}