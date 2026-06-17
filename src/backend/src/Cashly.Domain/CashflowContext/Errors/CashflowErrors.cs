using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CashflowContext.Errors;

public static class CashflowErrors
{
    public static readonly DomainError InvalidOwner = 
        new DomainError("Invalid.Owner", "Invalid owner id");
    
    public static readonly DomainError OwnerAlreadyExists = 
        new DomainError("Owner.Already.Exists", "The owner already exists");
    
    public static readonly DomainError MonthIsClosed =
        new DomainError("MonthIs.Closed", "Month is closed");

    public static readonly DomainError ScheduledTransactionsCannotBeClosed =
        new DomainError("Month.ScheduledTransactions", "Month cannot be closed with scheduled transactions.");
    
    public static readonly DomainError InvalidMember =
        new DomainError("Invalid.Member", "Invalid member");
    
    public static readonly DomainError PermissionDenied =
        new DomainError("Permission.Denied", "Permission denied");
}
