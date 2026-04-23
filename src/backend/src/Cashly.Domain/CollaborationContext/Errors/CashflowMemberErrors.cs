using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CollaborationContext.Errors;

public static class CashflowMemberErrors
{
    public static readonly DomainError CashflowReferenceRequired = new DomainError("Cashflow.Reference.Required", "Cashflow reference is required.");
    public static readonly DomainError UserReferenceRequired = new DomainError("User.Reference.Required", "User reference is required.");
    public static readonly DomainError InvalidCashflowMemberRole = new DomainError("Invalid.CashflowMemberRole", "Invalid role.");
    
}