using Cashly.Domain.CollaborationContext.Enums;
using Cashly.Domain.CollaborationContext.Errors;
using Cashly.Domain.Shared.Entities;
using Cashly.Domain.Shared.Exceptions;

namespace Cashly.Domain.CollaborationContext.Entities;

public sealed class CashflowMember : Entity
{
    public Guid CashflowId { get; private set; }
    public Guid UserId { get; private set; }
    public CashflowMemberRole Role { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    
    private CashflowMember() {}

    private CashflowMember(Guid cashflowId, Guid userId, CashflowMemberRole role)
    {
        CashflowId = cashflowId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTimeOffset.Now;
    }

    private static CashflowMember Create(Guid cashflowId, Guid userId, CashflowMemberRole role)
    {
        Validate(cashflowId, userId, role);
        return new CashflowMember(cashflowId, userId, role);
    }

    private static void Validate(Guid cashflowId, Guid userId, CashflowMemberRole role)
    {
        DomainExceptionValidation.When(cashflowId == Guid.Empty, CashflowMemberErrors.CashflowReferenceRequired);
        DomainExceptionValidation.When(userId == Guid.Empty, CashflowMemberErrors.UserReferenceRequired);
        DomainExceptionValidation.When(!Enum.IsDefined(role), CashflowMemberErrors.InvalidCashflowMemberRole);
    }

    internal static CashflowMember CreateOwner(Guid cashflowId, Guid userId)
        => Create(cashflowId, userId, CashflowMemberRole.Owner);
    
    internal static CashflowMember CreateContributor(Guid cashflowId, Guid userId)
        => Create(cashflowId, userId, CashflowMemberRole.Contributor);
    
    internal static CashflowMember CreateViewer(Guid cashflowId, Guid userId)
        => Create(cashflowId, userId, CashflowMemberRole.Viewer);
    
}