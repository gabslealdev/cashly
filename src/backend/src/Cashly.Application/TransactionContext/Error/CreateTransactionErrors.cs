using Cashly.Application.Shared.Results;

namespace Cashly.Application.TransactionContext.Error;


public static class CreateTransactionErrors
{
    public static readonly ApplicationError CashflowNotFound = new ApplicationError(
        "Cashflow.Not.Found", "Cashflow not found");
    
    public static readonly ApplicationError PermissionDenied = new ApplicationError(
        "Permission.Denied", "Permission denied, you can't create a transaction");
    
    public static readonly ApplicationError InvalidType = new ApplicationError(
        "Invalid.Type", "Invalid type");
    
    public static readonly ApplicationError InvalidStatus = new ApplicationError(
        "Invalid.Status", "Invalid status");
}
