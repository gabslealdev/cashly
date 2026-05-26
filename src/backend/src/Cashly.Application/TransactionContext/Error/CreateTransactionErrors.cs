using Cashly.Application.Shared.Results;

namespace Cashly.Application.TransactionContext.Error;


public static class CreateTransactionErrors
{
    public static readonly Shared.Results.Error PermissionDenied = new Shared.Results.Error(
        "Permission.Denied", "Permission denied, you can't create a transaction");
    
    public static readonly Shared.Results.Error InvalidType = new Shared.Results.Error(
        "Invalid.Type", "Invalid type");
    
    public static readonly Shared.Results.Error InvalidStatus = new Shared.Results.Error(
        "Invalid.Status", "Invalid status");
}