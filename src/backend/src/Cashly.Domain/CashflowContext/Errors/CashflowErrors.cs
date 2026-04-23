using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CashflowContext.Errors;

public static class CashflowErrors
{
    public static readonly DomainError InvalidOwner = new DomainError("InvalidOwner", "Invalid owner id");
    public static readonly DomainError OwnerAlreadyExists = new DomainError("Owner.Already.Exists", "The owner already exists");
}